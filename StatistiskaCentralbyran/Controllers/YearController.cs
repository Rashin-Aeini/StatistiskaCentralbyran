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
    public class YearController : ControllerBase
    {
        private IYearService YearService { get; }

        public YearController(IYearService yearService)
        {
            YearService = yearService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginateRequest request)
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
    }
}
