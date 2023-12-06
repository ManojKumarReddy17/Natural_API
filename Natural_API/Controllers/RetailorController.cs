﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;


namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailorController : ControllerBase
    {
        private readonly IRetailorService _retailorservice;
        private readonly IMapper _mapper;
        public RetailorController(IRetailorService retailorservice, IMapper mapper)
        {
            _retailorservice = retailorservice;
            _mapper = mapper;

        }

        // Get All Retailors

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetailorResource>>> GetRetailors()
        {
            var retailor = await _retailorservice.GetAllRetailors();
            var retailorResource = _mapper.Map<IEnumerable<Retailor>, IEnumerable<RetailorResource>>(retailor);
            return Ok(retailorResource);
        }

        // Get Retailor by Id

        [HttpGet("{id}")]

        public async Task<ActionResult<RetailorResponce>> GetByIdRetailor(string id)
        {
            var retailor = await _retailorservice.GetRetailorById(id);
            var retailorResource = _mapper.Map<Retailor, RetailorResource>(retailor);
            return Ok(retailorResource);
        }

        // Create Distributor

        [HttpPost]
        public async Task<ActionResult<RetailorResponce>> InsertRetailorWithAssociations([FromBody] RetailorResource retailorResource)
        {

            var retailor = _mapper.Map<RetailorResource, Retailor>(retailorResource);
            var createretailorResponse = await _retailorservice.CreateRetailorWithAssociationsAsync(retailor);
            return StatusCode(createretailorResponse.StatusCode, createretailorResponse);
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return Ok();
        }        

    }
}


