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
        private readonly IAssignRetailorToDistributorService _retailortodistributorservice;
        private readonly IAssignDistributorToExecutiveService _distributorToExecutiveService;

        private readonly IMapper _mapper;
        public DsrController(IDsrService dsrservice, IMapper mapper, IAssignRetailorToDistributorService retailortodistributorservice, IAssignDistributorToExecutiveService distributorToExecutiveService)
        {
            _dsrservice = dsrservice;
            _mapper = mapper;
            _retailortodistributorservice = retailortodistributorservice;
            _distributorToExecutiveService = distributorToExecutiveService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DsrResource>>> GetDsrList([FromQuery] DsrDetailsByIdResource? search)
        {
            var mapped = _mapper.Map<DsrDetailsByIdResource, EdittDSR>(search);
            var dsrs = await _dsrservice.GetAllDsr(mapped);
            var DsrList = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DsrResource>>(dsrs);
            return Ok(DsrList);
        }


        [HttpGet("Details/{ExecutiveId}")]
        public async Task<ActionResult<IEnumerable<DsrDistributorResource>>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {

            var result = await _dsrservice.AssignedDistributorDetailsByExecutiveId(ExecutiveId);
            var mapped = _mapper.Map<IEnumerable<DsrDistributor>, IEnumerable<DsrDistributorResource>>(result);

            return Ok(mapped);
        }
        [HttpGet("ById/{DsrId}")]

        public async Task<ActionResult<DsrRetailorResource>> GetDsrByDsrId(string DsrId)
        {
            var dsr = await _dsrservice.GetDsrbyId(DsrId);
          var dsrdetails =   await _dsrservice.GetDsrDetailsByDsrIdAsync(DsrId);
            
             var result =   _mapper.Map<Dsr, DsrInsertResource>(dsr);

            
            var details = _mapper.Map<List<DsrProduct>, List<DsrdetailProduct>>((List<DsrProduct>)dsrdetails);

            result.product = (details);
    
            return Ok(result);
        }


        [HttpGet("ids/{DsrId}")]
       

        public async Task<ActionResult<DsrEditResource>> GetByDsrId(string DsrId)
        {
            var dsr = await _dsrservice.GetbyId(DsrId);
            var result = _mapper.Map<Dsr, DsrEditResource>(dsr);
            var dsrdet = await _dsrservice.GetDetTableByDsrIdAsync(DsrId);
            var details = _mapper.Map<List<GetProduct>, List<DsrProductResource>>((List<GetProduct>)dsrdet);
            result.dsrdetail = (details);
            return Ok(result);

        }

        [HttpPost]
        public async Task<ActionResult<ResultResponse>> Insertdsr([FromBody] DsrInsertResource dsrResource)
        {
            var dsrdata = _mapper.Map<DsrInsertResource, Dsr>(dsrResource);
           var productlist= dsrResource.product;
           var drsdetaildata = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(productlist);
          
            var creadted = await _dsrservice.CreateDsrWithAssociationsAsync(dsrdata, drsdetaildata);

            return StatusCode(creadted.StatusCode, creadted);

        }

        
       
       
               [HttpPut("{DsrId}")]
        public async Task<ActionResult<ResultResponse>> updatedsr(string DsrId, [FromBody] DsrInsertResource dsrResource)
        {
            var dsrdata = _mapper.Map<DsrInsertResource, Dsr>(dsrResource);
            var productlist = dsrResource.product;
            dsrdata.Id = DsrId;
            var drsdetaildata = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(productlist);

            var creadted = await _dsrservice.UpdateDsrWithAssociationsAsync(dsrdata, drsdetaildata);

            return StatusCode(creadted.StatusCode, creadted);

        }


        [HttpDelete("{dsrId}")]
        public async Task<ActionResult<DsrResponse>> DeleteDsr(string dsrId)
        {

           
            var dsr = await _dsrservice.GetDsrbyId(dsrId);
            var dsrdetails = await _dsrservice.GetDsrDetailsByDsrIdAsync(dsrId);

            var drsdetaildata = _mapper.Map<List<DsrProduct>, List<Dsrdetail>>((List<DsrProduct>)dsrdetails);

            var response = await _dsrservice.DeleteDsr(dsr, drsdetaildata, dsrId);
            return Ok(response);
        }


  


        [HttpGet("AssignedDetails/{id}")]

        public async Task<ActionResult<IEnumerable<DsrRetailorResource>>> GetAssignedDetails(string id)
        {

            var distributorDetails = await _dsrservice.AssignedDistributorDetailsByExecutiveId(id);

            var mappedRetailers = new List<DsrRetailorResource>();

            foreach (var distributor in distributorDetails)
            {
                var distributorId = distributor.Id;

                var assignedRetailer = await _dsrservice.GetAssignedRetailorDetailsByDistributorId(distributorId);
                if (assignedRetailer != null)
                {
                    mappedRetailers = (List<DsrRetailorResource>)_mapper.Map<IEnumerable<DsrRetailor>, IEnumerable<DsrRetailorResource>>(assignedRetailer);


                }
            }



            return Ok(mappedRetailers);


        }

        [HttpGet("RetailorDetails/{id}/{date}")]
        public async Task<ActionResult<IEnumerable<DSRRetailorsListResource>>> GetDsrBydate(string id, DateTime date)
        {
            var retailorsList = await _dsrservice.GetRetailorListByDate(id, date); // Ensure this method filters by DSR created date
            var retailorDetails = await _retailortodistributorservice.GetRetailorsDetailsByDistributorId(id);
            var retailors = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DSRRetailorsListResource>>(retailorsList);
            foreach (var retailor in retailorDetails)
            {
                string fullname = string.Concat(retailor.FirstName + retailor.LastName);
                foreach (var retdetail in retailors)
                {
                    if (retdetail.Retailor == fullname)
                    {
                        retdetail.Address = retailor.Address;
                        retdetail.Phonenumber = retailor.MobileNumber;
                        retdetail.Image = retailor.Image;
                        retdetail.Area = retailor.Area;
                    }
                }
            }
            return Ok(retailors);
        }





        //[HttpGet("RetailorDetails/{distributorId}/{date}")]
        //public async Task<ActionResult<IEnumerable<DSRRetailorsListResource>>> GetDsrBydate(string distributorId, DateTime date)
        //{
        //    var retailorsList = await _dsrservice.GetRetailorListByDate(distributorId, date);
        //    var retailorDetails = await _retailortodistributorservice.GetRetailorsDetailsByDistributorId(distributorId);
        //    var retailors = _mapper.Map<IEnumerable<Dsr>, IEnumerable<DSRRetailorsListResource>>(retailorsList);
        //    foreach (var retailor in retailorDetails)
        //    {
        //        string fullname = string.Concat(retailor.FirstName + retailor.LastName);
        //        foreach (var retdetail in retailors)
        //        {
        //            if (retdetail.Retailor == fullname)
        //            {
        //                retdetail.Address = retailor.Address;
        //                retdetail.Phonenumber = retailor.MobileNumber;
        //            }
        //        }
        //    }
        //    return Ok(retailors);
        //}




        [HttpGet("RetailorDetailsbyExeOrDisId")]
        public async Task<ActionResult<IEnumerable<DSRretailorDetails>>> GetRetailorDetailsByExe(string Id)
        {

            var dsrrestils = await _dsrservice.GetDetailsByIdAsync(Id);
            return Ok(dsrrestils);

        }

    }
}