using StatistiskaCentralbyran.Models.ViewModels.Population;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Models.Interfaces
{
    public interface IPopulationService
    {
        Task<PopulationResponse> YearAsync(PopulationRequest entry);
        Task<PopulationResponse> RegionAsync(PopulationRequest entry);
        Task<PopulationResponse> GenderAsync(PopulationRequest entry);
    }
}
