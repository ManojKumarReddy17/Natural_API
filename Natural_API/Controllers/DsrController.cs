using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
<<<<<<< Updated upstream
using System.Globalization;
=======
using Natural_Services;
>>>>>>> Stashed changes

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DsrController : ControllerBase
    {
<<<<<<< Updated upstream

        private readonly IDSRService _dsrservice;
        private readonly IMapper _mapper;
        public DsrController(IDSRService dsrservice, IMapper mapper)
        {
            _dsrservice = dsrservice;
=======
        private readonly IDsrService _DsrService;
        private readonly IMapper _mapper;
        public DsrController(IDsrService DsrService, IMapper mapper)
        {
            _DsrService = DsrService;
>>>>>>> Stashed changes
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DSRResource>>> GetDsrList()
        {
<<<<<<< Updated upstream
            var dsrs = await _dsrservice.GetAllDsr();
            var DsrList = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DSRResource>>(dsrs);
=======
            var dsrs = await _DsrService.GetAllDsr();
            var DsrList = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DsrResource>>(dsrs);
>>>>>>> Stashed changes
            return Ok(DsrList);
        }


        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertDsrWithAssociations([FromBody] DsrPostResource dsrResource)
        {
<<<<<<< Updated upstream
            var response = _mapper.Map<DsrPostResource, Dsr>(dsrResource);
            var creadted = await _dsrservice.CreateDsrWithAssociationsAsync(response);
=======
            var dsrModel = _mapper.Map<Dsr>(dsrResource);
>>>>>>> Stashed changes


            var response = await _DsrService.CreateDsrWithAssociationsAsync(dsrModel);

            return StatusCode(response.StatusCode, response);
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
<<<<<<< Updated upstream
}


       








































       
=======
}
>>>>>>> Stashed changes
