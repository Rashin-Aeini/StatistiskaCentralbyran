using StatistiskaCentralbyran.Models.ViewModels.Pagination;
using StatistiskaCentralbyran.Models.ViewModels.Year;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Interfaces
{
    public interface IYearService
    {
        Task<YearResponse> ListAsync(PaginateRequest entry);
    }
}
