using API.Data.Entities;
using API.Data.Models;
using GAE.AIQ.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Data
{
    //Create and use abstrac class for reduce code lines writing and improves time of development.
    public abstract class Repository<TEntity, TContext> : IRepository<TEntity>
            where TEntity : class, IEntity
            where TContext : ApplicationDbContext
    {
        private readonly TContext context;
        public Repository(TContext context)
        {
            this.context = context;
        }
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> Delete(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return entity;
            }
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Get(int id, string includeProperties = "")
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            query = query.Where(x => x.Id == id);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {

            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<QueryResult<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             string includeProperties = "",
             int pageSize = 10,
             int pageIndex = 0)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            var pagedResult = new QueryResult<TEntity>();
            var result = new List<TEntity>();

            if (orderBy != null)
            {
                result = orderBy(query).ToList();
            }
            else
            {
                result = await query.ToListAsync();
            }

            pagedResult.TotalRecords = result.Count();
            pagedResult.CurrentPage = pageIndex + 1;
            pagedResult.PageSize = pageSize;
            pagedResult.Data = result.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return pagedResult;
        }

    }
}