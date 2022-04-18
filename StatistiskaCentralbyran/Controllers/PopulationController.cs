using Microsoft.AspNetCore.Mvc;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.ViewModels.Population;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PopulationController : ControllerBase
    {
        private IPopulationService PopulationService { get; }

        public PopulationController(IPopulationService populationService)
        {
            PopulationService = populationService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> YearAsync([FromQuery]PopulationRequest request)
        {
            try
            {
                return Ok(await  PopulationService.YearAsync(request));
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
            }
            
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> RegionAsync([FromQuery] PopulationRequest request)
        {
            try
            {
                return Ok(await PopulationService.RegionAsync(request));
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GenderAsync([FromQuery] PopulationRequest request)
        {
            try
            {
                return Ok(await PopulationService.GenderAsync(request));
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
