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
    public class DsrDetailController : ControllerBase
    {
        private readonly IDSRdetailsService _dsrdetails;
        private readonly IMapper _mapper;
        public DsrDetailController(IDSRdetailsService dSRdetailsService,IMapper mapper)
        {
            _dsrdetails = dSRdetailsService;
            _mapper = mapper;
            
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DsrDetailResource>>> GetDsrDetail()
        {
            var dsrdetail = await _dsrdetails.GetAllDsrdetail();
            var dsrdetaillist = _mapper.Map<IEnumerable<Dsrdetail>, IEnumerable<DsrDetailResource>>(dsrdetail);
            return Ok(dsrdetaillist);
        }
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertdsrdetailWithAssociations([FromBody] DsrDetailPostResource dsrDetailPostResource )
        {
            var response = _mapper.Map<DsrDetailPostResource, Dsrdetail>(dsrDetailPostResource);
            var creadted = await _dsrdetails.CreateDsrDetailsWithAssociationsAsync(response);

            return StatusCode(creadted.StatusCode, creadted);
        }
    }
}
