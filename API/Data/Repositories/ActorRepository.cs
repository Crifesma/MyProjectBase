using API.Data.Entities;
using GAE.AIQ.API.Data;

namespace API.Data.Repositories
{
	public class ActorRepository : Repository<Actor, ApplicationDbContext>
	{
		public ActorRepository(ApplicationDbContext context) : base(context)
		{

		}

	}
}
