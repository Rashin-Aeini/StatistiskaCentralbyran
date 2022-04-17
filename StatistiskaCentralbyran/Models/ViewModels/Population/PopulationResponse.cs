using System.Collections.Generic;

namespace StatistiskaCentralbyran.Models.ViewModels.Population
{
    public class PopulationResponse
    {
        public int TotalRecordes { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<PopulationViewModel> Recordes { get; set; }
    }
}
