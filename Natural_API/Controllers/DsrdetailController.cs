using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860



namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DsrdetailController : ControllerBase
    {

        private readonly IDsrdetailService _DsrdetailService;
        private readonly IMapper _mapper;
        public DsrdetailController(IDsrdetailService DsrdetailService, IMapper mapper)
        {
            _DsrdetailService = DsrdetailService;
            _mapper = mapper;

        }



        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<DsrProduct>>> GetDsrDetailsByDsrIdAsync(string dsrId)
        {

            var result = await _DsrdetailService.GetDsrDetailsByDsrIdAsync(dsrId);

            return Ok(result);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]

        // first image is being uploaded to s3bucket and later product data to mysql
        public async Task<ActionResult<ResultResponse>> InsertDsrdetails([FromBody] List<DsrdetailProduct> Dsrdetail, string id)
        {
            var detaiils = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(Dsrdetail);
            var createDistributorResponse = await _DsrdetailService.CreateDsrdetail(detaiils, id);

            return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        }


        [HttpPut]
        //[ValidateAntiForgeryToken]


        public async Task<ActionResult<ResultResponse>> updateDsrdetails([FromBody] List<DsrdetailProduct> Dsrdetail, string id)
        {
    
                var detaiils = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(Dsrdetail);

                var createDistributorResponse = await _DsrdetailService.UpadateDsrdetail(detaiils, id);

                return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
              
        }

       

        [HttpGet("ById/{dsrId}")]
        public async  Task<ActionResult<IEnumerable<GetProduct>>> GetDetailTableAsync(string dsrId)
        {

         var dsrrestils =   await _DsrdetailService.GetDetailTableAsync(dsrId);
            return Ok(dsrrestils);

        }

        

    }
}

