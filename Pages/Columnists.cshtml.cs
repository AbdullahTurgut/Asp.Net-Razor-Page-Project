using AdayRazorProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nest;

namespace AdayRazorProject.Pages
{
    public class ColumnistsModel : PageModel
    {
        public List<Columnist> _Columnists { get; set; } = new List<Columnist>();
        public List<Columnist> FilteredData { get; set; } = new List<Columnist>();

		public void OnGet(string searchTerm)
        {
            var response = GetResponse();
            _Columnists = response.Documents.ToList();

			if (!string.IsNullOrEmpty(searchTerm))
			{
				FilteredData = _Columnists.Where(item => item.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                         item.title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
			}
			else
			{
				FilteredData = _Columnists;
			}
		}

        public ISearchResponse<Columnist> GetResponse()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                            .DefaultIndex("crawling_data");

            var client = new ElasticClient(settings);

            var response = client.Search<Columnist>(s => s.Query(q => q
                        .MatchAll()
                    ));

            return response;
        }
    }
}
