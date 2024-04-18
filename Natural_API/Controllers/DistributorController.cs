using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3_Models;
using Natural_Services;
using System.Net.WebSockets;

#nullable disable

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorController : ControllerBase
    {
        private readonly IDistributorService _DistributorService;
        private readonly IMapper _mapper;
        public DistributorController(IDistributorService DistributorService, IMapper mapper)
        {
            _DistributorService = DistributorService;
            _mapper = mapper;

        }

        /// <summary>
        ///GETTING LIST OF DISTRIBUTOR DETAILS
        /// </summary>

        [HttpGet]
        public async Task<IEnumerable<GetDistributor>> GetAllDistributorDetails(string? prefix)
        {
            var executive = await _DistributorService.GetAllDistributorDetailsAsync(prefix);
            return executive;
        }

        /// <summary>
        ///GETTING LIST OF NON-ASSIGNED DISTRIBUTORS DETAILS
        /// </summary>

        [HttpGet("Assign")]
        public async Task<ActionResult<IEnumerable<DistributorGetResource>>> GetNonAssignedDistributorDetails()
        {
            var distributors = await _DistributorService.GetNonAssignedDistributors();
            var distributorResources = _mapper.Map<IEnumerable<Distributor>, IEnumerable<DistributorGetResource>>(distributors);
            return Ok(distributorResources);
        }

        /// <summary>
        /// GETTING DISTRIBUTOR BY ID
        /// </summary>


        [HttpGet("{DisId}")]
        public async Task<ActionResult<Distributor>> GetDistributorById(string DisId)
        {
            var distributor = await _DistributorService.GetDistributorPresignedUrlbyId(DisId);
            return Ok(distributor);
        }

        /// <summary>
        ///GETTING DISTRIBUTOR DETAILS BY ID
        /// </summary>

        [HttpGet("Details/{DistributorId}")]

        public async Task<ActionResult<DistributorGetResource>> GetDistributorDetailsById(string DistributorId)
        {
            var distributor = await _DistributorService.GetDistributorDetailsById(DistributorId);
            var distributorResource = _mapper.Map<Distributor, DistributorGetResource>(distributor);
            return Ok(distributorResource);
        }

        /// <summary>
        /// CREATING NEW DISTRIBUTOR
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertDistributorWithAssociations([FromForm] InsertUpdateResource distributorResource, string? prefix)
        {
            var file = distributorResource.UploadImage;
            var result = await _DistributorService.UploadFileAsync(file, prefix);
            var distributor = _mapper.Map<InsertUpdateResource, Distributor>(distributorResource);
            distributor.Image = result.Message;
            var createDistributorResponse = await _DistributorService.CreateDistributorWithAssociationsAsync(distributor);
            return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        }

        /// <summary>
        /// UPDATING DISTRIBUTOR BY ID
        /// </summary>

        [HttpPut]
        public async Task<ActionResult<InsertUpdateResource>> UpdateDistributor(string DistributorId, [FromForm] InsertUpdateResource updatedistributor, string? prefix)
        {

            var ExistingDistributor = await _DistributorService.GetDistributorById(DistributorId);

            var file = updatedistributor.UploadImage;
            if (file != null && file.Length > 0)
            {
                var result = await _DistributorService.UploadFileAsync(file, prefix); //change uploadfile to image
                var mappeddis = _mapper.Map(updatedistributor, ExistingDistributor);
                mappeddis.Image = result.Message;
                var Updateresponse = await _DistributorService.UpdateDistributor(mappeddis);
                return StatusCode(Updateresponse.StatusCode, Updateresponse);
            }



            var distributorToUpdate = _mapper.Map(updatedistributor, ExistingDistributor);
            var update = await _DistributorService.UpdateDistributor(distributorToUpdate);

            return StatusCode(update.StatusCode, update);

        }


        /// <summary>
        /// DELETING DISTRIBUTOR BY ID
        /// </summary>

        [HttpDelete("{DistributorId}")]
        public async Task<ActionResult<ResultResponse>> DeleteDistributor(string DistributorId)
        {
            var response = await _DistributorService.SoftDelete(DistributorId);

            return response;
        }


        /// <summary>
        /// SEARCH DISTRIBUTOR 
        /// </summary>

        [HttpPost("Search")]
        public async Task<IEnumerable<DistributorGetResource>> SearchDistributor([FromBody] SearchModel search)
        {
            var exe = await _DistributorService.SearchDistributors(search);
            var execget = _mapper.Map<IEnumerable<Distributor>, IEnumerable<DistributorGetResource>>(exe);
            return execget;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<AngularLoginResponse>> Login([FromBody] AngularLoginResourse loginModel)
        {
            var credentials = _mapper.Map<AngularLoginResourse, Distributor>(loginModel);
            var user = await _DistributorService.LoginAsync(credentials);

            return StatusCode(user.Statuscode, user);
        }

            [HttpPost("SearchNonAssign")]
            public async Task<IEnumerable<DistributorGetResource>> SearchNonAssignDistributor([FromBody] SearchModel SearchNonAssign)
            {
                var exe = await _DistributorService.SearchNonAssignedDistributors(SearchNonAssign);
                var execget = _mapper.Map<IEnumerable<Distributor>, IEnumerable<DistributorGetResource>>(exe);
                return execget;
            }

        }

}





