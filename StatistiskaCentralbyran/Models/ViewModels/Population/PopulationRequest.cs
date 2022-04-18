using StatistiskaCentralbyran.Models.ViewModels.Pagination;

namespace StatistiskaCentralbyran.Models.ViewModels.Population
{
    public class PopulationRequest : PaginateRequest
    {
        public string Value { get; set; }
    }
}
