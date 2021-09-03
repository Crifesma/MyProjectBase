using API.Data.Entities;
using API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Data
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id, string includeProperties = "");
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);

        Task<QueryResult<T>> Get(Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "",
           int pageSize = 10,
           int pageIndex = 0);
    }
}
