using GAE.AIQ.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Data
{
	public class DataBaseFactory : IDataBaseFactory
	{
		private ApplicationDbContext dataContext;
		private readonly IConfiguration _configuration;
		private string _connectionString;
		DbContextOptionsBuilder<ApplicationDbContext> _optionsBuilder;

		public DataBaseFactory(IConfiguration configuration)
		{
			_configuration = configuration;
			_optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			_connectionString = _configuration.GetConnectionString("AIQDB");
			_optionsBuilder.UseSqlServer(_connectionString);
		}

		public ApplicationDbContext GetContext()
		{
			dataContext = new ApplicationDbContext(_optionsBuilder.Options, _configuration);
			return dataContext;
		}
		public void Dispose()
		{
			if (dataContext != null)
				dataContext.Dispose();
		}
	}
}
