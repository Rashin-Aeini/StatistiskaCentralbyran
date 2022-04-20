using Microsoft.AspNetCore.Mvc;
using StatistiskaCentralbyran.Models.Domains;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.ViewModels.Pagination;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private IYearService YearService { get; }
        private IRegionService RegionService { get; }

        public ValueController(IYearService yearService, IRegionService regionService)
        {
            YearService =yearService;
            RegionService = regionService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Year(PaginateRequest request)
        {
            try
            {
                return Ok(await YearService.ListAsync(request));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Region(PaginateRequest request)
        {
            try
            {
                return Ok(await RegionService.ListAsync(request));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("[action]")]
        public IActionResult Gender(PaginateRequest request)
        {
            return Ok(Enum.GetNames(typeof(Gender)).ToList());
        }
    }
}
