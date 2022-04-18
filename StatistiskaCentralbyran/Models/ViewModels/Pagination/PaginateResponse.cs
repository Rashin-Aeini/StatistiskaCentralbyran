using System.Collections.Generic;

namespace StatistiskaCentralbyran.Models.ViewModels.Pagination
{
    public abstract class PaginateResponse<T> where T : class
    {
        public int TotalRecordes { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<T> Recordes { get; set; }
    }
}
