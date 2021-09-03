using API.Data;
using API.Data.Entities;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Controllers
{
    //also when in the repositoryes create Abstrac class.
    public class EntityControllerBase<TEntity, TRepository> : ControllerBase
                    where TEntity : class, IEntity
                    where TRepository : IRepository<TEntity>
    {

        private readonly TRepository _repository;
        protected string _getIncludedProperties = string.Empty;

        protected string _detailIncludedProperties = string.Empty;

        public EntityControllerBase(TRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        //[Authorize]
        public virtual async Task<ActionResult<QueryResult<TEntity>>> Get([FromQuery] QueryParameters queryParams)
        {
            Expression<Func<TEntity, bool>> filter = null;
            if (queryParams.searchProperty != null && queryParams.searchTerm != null)
            {
                ParameterExpression pe = Expression.Parameter(typeof(TEntity), "x");
                filter = Expression.Lambda<Func<TEntity, bool>>(ContainsFilterWithOr(pe, queryParams.searchTerm, queryParams.searchProperty), pe);
            }

            return await _repository.Get(filter, null, _getIncludedProperties, queryParams.pageSize, queryParams.currentPage - 1);
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        //[Authorize]
        public virtual async Task<ActionResult<TEntity>> Get(int id)
        {
            var movie = await _repository.Get(id, _detailIncludedProperties);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }


        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> Put(int id, TEntity entity)
        {

            if (id != entity.Id)
            {
                return BadRequest();
            }
            await _repository.Update(entity);
            return NoContent();
        }

        // POST: api/[controller]
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
            await _repository.Add(entity);
            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        //[Authorize]
        public virtual async Task<ActionResult<TEntity>> Delete(int id)
        {
            var movie = await _repository.Delete(id);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }

        [HttpPost("Filter")]
        //[Authorize]
        public async Task<ActionResult<QueryResult<TEntity>>> Filter(List<FilterData> filterDatas, [FromQuery] QueryParameters queryParams)
        {
            Expression<Func<TEntity, bool>> filter = null;
            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "x");
            Expression subFilter = ContainsFilterWithAnd(pe, filterDatas);
            if (subFilter != null)
                filter = Expression.Lambda<Func<TEntity, bool>>(subFilter, pe);
            return await _repository.Get(filter, null, _getIncludedProperties, queryParams.pageSize, queryParams.currentPage - 1);
        }

        #region GenericEntityFilter

        private Expression ContainsFilterWithAnd(ParameterExpression pe, List<FilterData> filterDatas)
        {
            List<Expression> ands = new List<Expression>();
            foreach (FilterData filter in filterDatas)
            {
                Expression left = pe;
                var member = filter.NameColumn;

                foreach (var nestedMember in member.Split("."))
                {
                    left = MemberExpression.Property(left, nestedMember);
                }

                ands.Add(Contains(left, filter.DataForFind));

            }

            Expression end = null;

            foreach (Expression oneAnd in ands)
            {
                end = end != null ? Expression.And(end, oneAnd) : oneAnd;
            }

            return end;
        }

        private Expression ContainsFilterWithOr(ParameterExpression pe, string searchTerm, string searchProperty)
        {

            Expression left = pe;
            var member = searchProperty.Split('-')[0];
            foreach (var nestedMember in member.Split("."))
            {
                left = MemberExpression.Property(left, nestedMember);
            }

            Expression containsMethodExp = Contains(left, searchTerm);

            if (containsMethodExp == null)
                return null;

            if (searchProperty.Split('-').Length > 1)
            {
                for (int i = 1; i < searchProperty.Split('-').Length; i++)
                {
                    var members = searchProperty.Split('-')[i];
                    Expression lefts = pe;
                    foreach (var nestedMember in members.Split("."))
                    {
                        lefts = MemberExpression.Property(lefts, nestedMember);
                    }

                    Expression otherE = Contains(lefts, searchTerm);
                    if (otherE == null)
                        continue;

                    Expression newE = Expression.OrElse(containsMethodExp, otherE);
                    containsMethodExp = newE;
                }

            }
            return containsMethodExp;
        }

        private Expression Contains(Expression left, string searchTerm)
        {
            Type typeS = left.Type;
            MethodInfo method = null;
            ConstantExpression right = null;
            if (typeS.Name == typeof(string).Name)
            {
                method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                right = Expression.Constant(searchTerm, typeof(string));
                return Expression.Call(left, method, right);
            }

            if (typeS.Name == typeof(int).Name)
            {
                int value = int.Parse(searchTerm);
                right = Expression.Constant(value, typeof(int));
                return Expression.Equal(left, right);
            }

            if (typeS.Name == typeof(bool).Name)
            {
                bool value = bool.Parse(searchTerm);
                right = Expression.Constant(value, typeof(bool));
                return Expression.Equal(left, right);
            }

            if (typeS.FullName.Contains(typeof(DateTime).Name))
            {
                left = Expression.Convert(left, typeof(DateTime));

                Expression day = MemberExpression.Property(left, "Day");
                Expression month = MemberExpression.Property(left, "Month");
                Expression year = MemberExpression.Property(left, "Year");

                DateTime value = DateTime.Parse(searchTerm);
                if (value.Year < 2000)
                    return null;

                Expression rDay = Expression.Constant(value.Day, typeof(int));
                Expression rMonth = Expression.Constant(value.Month, typeof(int));
                Expression rYear = Expression.Constant(value.Year, typeof(int));

                Expression compareDay = Expression.Equal(day, rDay);
                Expression compareMonth = Expression.Equal(month, rMonth);
                Expression compareYear = Expression.Equal(year, rYear);
                return Expression.And(Expression.And(compareDay, compareMonth), compareYear);
            }

            return null;
        }
        #endregion

    }
}