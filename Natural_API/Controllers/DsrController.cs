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
    public class DsrController : ControllerBase
    {

        private readonly IDSRService _repository;
        private readonly IMapper _mapper;
        public DsrController(IDSRService repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DSRResource>>> GetDsrList()
        {
            var dsrs = await _repository.GetAllDsr();
            var DsrList = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DSRResource>>(dsrs);
            return Ok(DsrList);
        }
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertdsrWithAssociations([FromBody] DsrPostResource dsrResource)
        {
            var response = _mapper.Map<DsrPostResource, Dsr>(dsrResource);
            var creadted = await _repository.CreateDsrWithAssociationsAsync(response);

            return StatusCode(creadted.StatusCode, creadted);

        }
        [HttpGet("Details/{dsrId}")]
        public async Task<ActionResult<DSRResource>> GetDsrDetailsByID(string dsrId)
        {
            var dsr=await _repository.GetDsrDetailsById(dsrId);
            var dsrresource=_mapper.Map<Dsr,DSRResource>(dsr);
            return dsrresource;

        }
        [HttpGet("{dsrId}")]
        public async Task<ActionResult<DSRResource>> GetDsrById(string dsrId)
        {
            var dsr = await _repository.GetDsrDetailsById(dsrId);
            var mapped = _mapper.Map<Dsr, DSRResource>(dsr);
            return mapped;


        }
    }
}
