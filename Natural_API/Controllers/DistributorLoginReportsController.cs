using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorLoginReportsController : ControllerBase
    {
        private readonly IDistributorLoginReportsService _distributorLoginReportsService;
        private readonly IMapper _mapper;
        public DistributorLoginReportsController(IDistributorLoginReportsService distributorLoginReportsService, IMapper mapper)
        {
            _distributorLoginReportsService = distributorLoginReportsService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<DistributorReport>> DistributorLoginReports([FromQuery] DistributorLoginReports search)
        {

            var salesReport = await _distributorLoginReportsService.getbyId(search);
            if (salesReport == null)
            {
                return NotFound();
            }
            return Ok(salesReport);
        }
        [HttpPost("search")]
        public async Task<ActionResult<DistributorReport>> DistributorLoginReportsSearch([FromBody] DistributorLoginReports search)
        {
            var salesReport = await _distributorLoginReportsService.getbyId(search);
            if (salesReport == null)
            {
                return NotFound();
            }
            return Ok(salesReport);
        }
    }
}
