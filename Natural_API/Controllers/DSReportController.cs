using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DSReportController : ControllerBase
    {
        private readonly IDistributorSalesService _DistributorSalesService;
        private readonly IMapper _mapper;

        public DSReportController(IDistributorSalesService distributorSalesService, IMapper mapper)
        {
            _DistributorSalesService = distributorSalesService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<DistributorSalesReport>> GetSalesReport([FromQuery] DistributorSalesReportInput dsr)
        {
            var salesReport = await _DistributorSalesService.GetById(dsr);
            if (salesReport == null)
            {
                return NotFound();
            }
            return Ok(salesReport);
        }

        [HttpPost]
        public async Task<ActionResult<DistributorSalesReport>> Search([FromBody] DistributorSalesReportInput search)
        {

            var salesReport = await _DistributorSalesService.GetById(search);
            if (salesReport == null)
            {
                return NotFound();
            }
            return Ok(salesReport);
        }




    }
}

