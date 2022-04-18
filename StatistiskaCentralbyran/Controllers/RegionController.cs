using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatistiskaCentralbyran.Models.Interfaces;
using StatistiskaCentralbyran.Models.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatistiskaCentralbyran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private IRegionService RegionService { get; }

        public RegionController(IRegionService regionService)
        {
            RegionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginateRequest request)
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
    }
}
