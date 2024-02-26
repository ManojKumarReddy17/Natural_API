using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;


namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DsrController : ControllerBase
    {

        private readonly IDsrService _dsrservice;
        private readonly IMapper _mapper;
        public DsrController(IDsrService dsrservice, IMapper mapper)
        {
            _dsrservice = dsrservice;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DsrResource>>> GetDsrList()

        {
            var dsrs = await _dsrservice.GetAllDsr();
            var DsrList = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DsrResource>>(dsrs);
            return Ok(DsrList);
        }


        [HttpGet("Product")]
        public async Task<ActionResult<DsrProductResource>> GetProductAsync()
        {
            var product = await _dsrservice.GetProductAsync();
            var mapped = _mapper.Map<IEnumerable<Product>, IEnumerable<DsrProductResource>>(product);

            return Ok(mapped);
        }


        [HttpGet("Details/{ExecutiveId}")]
        public async Task<ActionResult<IEnumerable<DsrDistributorResource>>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {

            var result = await _dsrservice.AssignedDistributorDetailsByExecutiveId(ExecutiveId);
            var mapped = _mapper.Map<IEnumerable<DsrDistributor>, IEnumerable<DsrDistributorResource>>(result);

            return Ok(mapped);
        }

        [HttpGet("{DistributorId}")]
        public async Task<ActionResult<IEnumerable<DsrRetailorResource>>> GetAssignedRetailorDetailsByDistributorId(string DistributorId)
        {

            var result = await _dsrservice.GetAssignedRetailorDetailsByDistributorId(DistributorId);
            var mapped = _mapper.Map<IEnumerable<DsrRetailor>, IEnumerable<DsrRetailorResource>>(result);

            return Ok(mapped);
        }

        [HttpGet("ById/{DsrId}")]
        // this method is for get dsr and dsrdetails by id

        public async Task<ActionResult<DsrRetailorResource>> GetDsrByDsrId(string DsrId)
        {
            var dsr = await _dsrservice.GetDsrbyId(DsrId);
          var dsrdetails =   await _dsrservice.GetDsrDetailsByDsrIdAsync(DsrId);
            
             var result =   _mapper.Map<Dsr, DsrInsertResource>(dsr);

            var details = _mapper.Map<List<Dsrdetail>, List<DsrdetailProduct>>((List<Dsrdetail>)dsrdetails);
           
            result.product = (details);
    
            return Ok(result);
        }





        [HttpPost("Search")]
        public async Task<ActionResult<IEnumerable<Dsr>>> SearchDsr([FromBody] DsrDetailsByIdResource search)

        {
           var mapped=  _mapper.Map<DsrDetailsByIdResource, Dsr>(search);

            var selut =  await _dsrservice.SearchDsr(mapped);
            return Ok(selut);
        }
    

        [HttpPost]
        public async Task<ActionResult<ResultResponse>> Insertdsr([FromBody] DsrInsertResource dsrResource)
        {
            var dsrdata = _mapper.Map<DsrInsertResource, Dsr>(dsrResource);
           var productlist=                dsrResource.product;
           var drsdetaildata = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(productlist);
          
            var creadted = await _dsrservice.CreateDsrWithAssociationsAsync(dsrdata, drsdetaildata);

            return StatusCode(creadted.StatusCode, creadted);

        }

       

        [HttpDelete("{dsrId}")]
        public async Task<ActionResult<DsrResponse>> DeleteDsr(String dsrId)
        {
            var response = await _dsrservice.DeleteDsr(dsrId);
            return Ok(response);
        }
    }
}