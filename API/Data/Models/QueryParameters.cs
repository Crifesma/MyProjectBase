
namespace API.Data.Models
{
	public class QueryParameters
	{
		public int currentPage { get; set; }
		public int pageSize { get; set; }
		public int year { get; set; }
		public int month { get; set; }
		public int day { get; set; }
		public string searchTerm { get; set; }
		public string searchProperty { get; set; }

		public QueryParameters()
		{
			currentPage = 1;
			pageSize = 10;
			searchTerm = null;
			searchProperty = null;
			year = 0;
			month = 0;
			day = 0;
		}
	}
}
