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
        private readonly IDsrService _repository;
        private readonly IMapper _mapper;
        public DsrController(IDsrService repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DsrResource>>> GetDsrList()
        {
            var dsrs = await _repository.GetAllDsr();
            var DsrList = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DsrResource>>(dsrs);
            return Ok(DsrList);
        }
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertdsrWithAssociations([FromBody] DsrPostResource dsrResource)
        {
            var response = _mapper.Map<DsrPostResource, Dsr>(dsrResource);
            var creadted = await _repository.CreateDsrWithAssociationsAsync(response);

            return StatusCode(creadted.StatusCode, creadted);

        }
    }
}
