using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using System.Globalization;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DsrController : ControllerBase
    {

        private readonly IDSRService _dsrservice;
        private readonly IMapper _mapper;
        public DsrController(IDSRService dsrservice, IMapper mapper)
        {
            _dsrservice = dsrservice;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DSRResource>>> GetDsrList()
        {
            var dsrs = await _dsrservice.GetAllDsr();
            var DsrList = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DSRResource>>(dsrs);
            return Ok(DsrList);
        }


        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertdsrWithAssociations([FromBody] DsrPostResource dsrResource)
        {
            var response = _mapper.Map<DsrPostResource, Dsr>(dsrResource);
            var creadted = await _dsrservice.CreateDsrWithAssociationsAsync(response);

            return StatusCode(creadted.StatusCode, creadted);

        }


        /// <summary>
        /// To get products and dsrdetails
        /// </summary>


        [HttpGet("Details/{dsrId}")]
        public async Task<ActionResult<DsrDetailsByIdResource>> GetDsrAndProductDetailsById(string dsrId)
        {
            var dsr = await _dsrservice.GetDsrDetailsById(dsrId);
            var dsrresource = _mapper.Map<Dsr, DsrDetailsByIdResource>(dsr);
            return dsrresource;
        }

        [HttpDelete("{dsrId}")]
        public async Task<ActionResult<DsrResponse>> DeleteDsr(String dsrId)
        {
            var response = await _dsrservice.DeleteDsr(dsrId);
            return Ok(response);
        }
    }
}