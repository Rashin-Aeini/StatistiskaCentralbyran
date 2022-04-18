using Microsoft.AspNetCore.Mvc;
using StatistiskaCentralbyran.Models.Domains;
using System;
using System.Linq;

namespace StatistiskaCentralbyran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(Enum.GetNames(typeof(Gender)).ToList());
        }
    }
}
