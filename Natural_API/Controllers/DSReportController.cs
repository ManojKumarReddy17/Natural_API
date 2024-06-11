using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Serilog;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DSReportController : ControllerBase
    {
        private readonly IDistributorSalesService _DistributorSalesService;
        private readonly IMapper _mapper;
        private readonly ILogger<DSReportController> _logger;

        public DSReportController(IDistributorSalesService distributorSalesService, IMapper mapper, ILogger<DSReportController> logger)
        {
            _DistributorSalesService = distributorSalesService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<DistributorSalesReport>> GetSalesReport([FromQuery] DistributorSalesReportInput dsr)
        {
            Log.Information("Starting GetSalesReport method with input: {DistributorSalesReportInput}", dsr);

            try
            {
                var salesReport = await _DistributorSalesService.GetById(dsr);
                if (salesReport == null)
                {
                    Log.Warning("No sales report found for input: {DistributorSalesReportInput}", dsr);
                    return NotFound();
                }

                Log.Information("Successfully retrieved sales report for input: {DistributorSalesReportInput}", dsr);
                return Ok(salesReport);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving sales report for input: {DistributorSalesReportInput}", dsr);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Information("Completed GetSalesReport method for input: {DistributorSalesReportInput}", dsr);
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<DistributorSalesReport>> Search([FromBody] DistributorSalesReportInput search)
        {
            Log.Information("Starting Search method with input: {DistributorSalesReportInput}", search);

            try
            {
                var salesReport = await _DistributorSalesService.GetById(search);
                if (salesReport == null)
                {
                    Log.Warning("No sales report found for input: {DistributorSalesReportInput}", search);
                    return NotFound();
                }

                Log.Information("Successfully retrieved sales report for input: {DistributorSalesReportInput}", search);
                return Ok(salesReport);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving sales report for input: {DistributorSalesReportInput}", search);
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                Log.Information("Completed Search method for input: {DistributorSalesReportInput}", search);
            }

           
        }




    }
}

