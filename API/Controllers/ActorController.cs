using API.Data.Entities;
using API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ActorController :
			EntityControllerBase<Movie, MovieRepository>
	{
		public ActorController(MovieRepository repository) : base(repository)
		{

		}
	}
}
