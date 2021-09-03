using API.Data.Entities;
using API.Data.Models;
using GAE.AIQ.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Data.Repositories
{
	public class MovieRepository : Repository<Movie, ApplicationDbContext>
	{
        private ApplicationDbContext _dbContext;
		public MovieRepository(ApplicationDbContext context) : base(context)
		{
            _dbContext = context;
		}

        //Create override by necessity of load the actor nested movie and improves performace  
        public override async Task<QueryResult<Movie>> Get(Expression<Func<Movie, bool>> filter = null,
             Func<IQueryable<Movie>, IOrderedQueryable<Movie>> orderBy = null,
             string includeProperties = "",
             int pageSize = 10,
             int pageIndex = 0)
        {
            IQueryable<Movie> query = _dbContext.Set<Movie>();

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

            var pagedResult = new QueryResult<Movie>();
            var result = new List<Movie>();

            if (orderBy != null)
            {
                result = orderBy(query).ToList();
            }
            else
            {
                result = await query.ToListAsync();
            }

            foreach (Movie movie in result)
            {
                List<int> actorIds = await _dbContext.MovieActors.Where(x => x.MovieId == movie.Id).Select(x=>x.ActorId).ToListAsync();
                movie.Actors = await _dbContext.Actors.Where(x => actorIds.Contains(x.Id)).ToListAsync();
            }

            pagedResult.TotalRecords = result.Count();
            pagedResult.CurrentPage = pageIndex + 1;
            pagedResult.PageSize = pageSize;
            pagedResult.Data = result.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return pagedResult;
        }


        //to can create methods for each additional case, the CRUD basic is already
        public async Task<QueryResult<Movie>> searchWithActorName(
             string searchTerm = "",
             string searchProperty = "",
             int pageSize = 10,
             int pageIndex = 0)
        {
            IQueryable<Movie> query = _dbContext.Set<Movie>();

            var pagedResult = new QueryResult<Movie>();
            var result = new List<Movie>();

            if (searchProperty == "Actor.Name")
            {
                result = await query.Where(x =>  x.Actors.Any(x => x.Name.Contains(searchTerm))).ToListAsync();
            }

            if (searchProperty== "Name-Actor.Name")
            {
                result = await query.Where(x => x.Name.Contains(searchTerm) || x.Actors.Any(x => x.Name.Contains(searchTerm))).ToListAsync();
            }

            if (searchProperty == "Genre-Actor.Name")
            {
                result = await query.Where(x => x.Genre.Contains(searchTerm) || x.Actors.Any(x => x.Name.Contains(searchTerm))).ToListAsync();
            }

            if (searchProperty == "Name-Genre-Actor.Name")
            {
                result = await query.Where(x => x.Name.Contains(searchTerm) || x.Genre.Contains(searchTerm) || x.Actors.Any(x => x.Name.Contains(searchTerm))).ToListAsync();
            }

            foreach (Movie movie in result)
            {
                List<int> actorIds = await _dbContext.MovieActors.Where(x => x.MovieId == movie.Id).Select(x => x.ActorId).ToListAsync();
                movie.Actors = await _dbContext.Actors.Where(x => actorIds.Contains(x.Id)).ToListAsync();
            }

            pagedResult.TotalRecords = result.Count();
            pagedResult.CurrentPage = pageIndex + 1;
            pagedResult.PageSize = pageSize;
            pagedResult.Data = result.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return pagedResult;
        }

        }
}
