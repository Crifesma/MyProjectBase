using API.Data.Entities;
using API.Data.Models;
using API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController :
            EntityControllerBase<Movie, MovieRepository>
    {
        private readonly MovieRepository _repository;
        public MovieController(MovieRepository repository) : base(repository)
        {
            _repository = repository;
        }

        //to can create methods (endPoints) for each additional case, the CRUD basic is already
        [HttpGet("SearchWithActorName")]
        public async Task<ActionResult<QueryResult<Movie>>> searchWithActorName([FromQuery] QueryParameters queryParams)
        {
            return await _repository.searchWithActorName(queryParams.searchTerm, queryParams.searchProperty, queryParams.pageSize, queryParams.currentPage);
        }
    }
}
