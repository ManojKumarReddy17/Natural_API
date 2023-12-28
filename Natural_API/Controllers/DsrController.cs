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


        //[HttpGet("{id}")]
        //public async Task<ActionResult<DSRResource>> GetDsr(string id)
        //{
        //    var dsr = await _dsrservice.GetDsrById(id);
        //    if (dsr != null)
        //    {
        //        var mapped = _mapper.Map<Dsr, DSRResource>(dsr);

        //        return Ok(mapped);
        //    }
        //    return BadRequest();
        //}


        [HttpGet("product/{dsrId}")]
        public async Task<ActionResult<IEnumerable<DsrProductResource>>> GetProductDetails(string dsrId)
        {
            var product = await _dsrservice.GetProductsByDsrIdAsync(dsrId);
            if (product != null)
            {
                var mapped = _mapper.Map<IEnumerable<Product>, IEnumerable<DsrProductResource>>(product);
                return Ok(mapped);
            }
            return NotFound();
        }

        [HttpGet("{dsrId}")]
        public async Task<ActionResult<DSRResource>> GetDsrAndProductDetails(string dsrId)
        {
            var dsr = await _dsrservice.GetAllDetails(dsrId);          
            var mappedDsr = _mapper.Map<Dsr, DSRResource>(dsr);
            var dsr = await _repository.GetDsrDetailsById(dsrId);
            var mapped = _mapper.Map<Dsr, DSRResource>(dsr);
            return mapped;


        }

    
































        //public async Task<ActionResult<IEnumerable<DsrProductResource>>> GetProduct(string id)
        //{

        //    var product = await _dsrservice.GetProductsByDsrIdAsync(id);
        //    if (product != null)
        //    {
        //        var mapped = _mapper.Map<IEnu, DsrProductResource>(product);
        //        return Ok(mapped);
        //    }

        //    return Ok(mapped);

        //}

    }
}










        //[HttpGet("{dsrId}")]
        //public async Task<ActionResult<DSRResource>> GetDsrDetailsByID(string dsrId)
        //{

        //    var dsr = await _dsrservice.GetDsrDetailsById(dsrId);
        //    var dsrresource = _mapper.Map<Dsr, DSRResource>(dsr);
        //    return dsrresource;

        //}


        //[HttpGet("GetProductsByDsrId/{dsrId}")]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProductsByDsrId(string dsrId)
        //{
        //    var products = await _dsrservice.GetDsrDetailsById(dsrId);

        //    if (products == null )
        //    {
        //        return NotFound("No products found for the given DsrId.");
        //    }

        //    return Ok(products);
        //}

    



















        //[HttpGet("{dsrId}")]
        //public async Task<ActionResult<DSRResource>> GetDsrById(string dsrId)
        //{

        //    var dsr = await _dsrservice.GetDsrById(dsrId);
        //    var mapped = _mapper.Map<Dsr, DSRResource>(dsr);
        //    return mapped;

        //}

        //[HttpDelete("{dsrId}")]
        //public async Task<ActionResult<DsrResponse>> DeleteDsr(String dsrId)
        //{
        //    var response=await _dsrservice.DeleteDsr(dsrId);
        //    return  Ok(response);
        //}
    

