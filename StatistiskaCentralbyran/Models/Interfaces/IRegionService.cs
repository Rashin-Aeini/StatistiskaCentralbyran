using StatistiskaCentralbyran.Models.ViewModels.Pagination;
using StatistiskaCentralbyran.Models.ViewModels.Region;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Interfaces
{
    public interface IRegionService
    {
        Task<RegionResponse> ListAsync(PaginateRequest entry);
    }
}
