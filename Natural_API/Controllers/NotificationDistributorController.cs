using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3Models;
using Natural_Services;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationDistributorController : ControllerBase
    {
        private readonly INotificationDistributorService _NotificationDistributorService;
        private readonly IMapper _mapper;

        public NotificationDistributorController(INotificationDistributorService NotificationDistributorService, IMapper mapper)
        {
            _NotificationDistributorService = NotificationDistributorService;
            _mapper = mapper;

        }



        [HttpGet]
        // GET: /<controller>/
        public async Task<ActionResult<IEnumerable<NotificationResource>>> GetNotification()

        {
            var notifiactions = await _NotificationDistributorService.GetAll();

            var notifydata = _mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationResource>>(notifiactions);
            return Ok(notifydata);
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<NotificationResource>> GetNotificationById(string Id)
        {
            var notification = await _NotificationDistributorService.GetNotificationByIdAsync(Id);
            var distributorlist = await _NotificationDistributorService.GetDistributorsByNotificationIdAsync(Id);
            var executivelist = await _NotificationDistributorService.GetExecutivesByNotificationIdAsync(Id);
            var exedata = _mapper.Map<List<NotificationExecutive>, List<NotificationExecutiveResource>>((List<NotificationExecutive>)executivelist);
            var drsdetaildata = _mapper.Map<List<NotificationDistributor>, List<NotificationDistributorResource>>((List<NotificationDistributor>)distributorlist);
            var notificationdata = _mapper.Map<Notification, NotificationResource>(notification);
            notificationdata.distributorlist = drsdetaildata;
            notificationdata.executiveList = exedata;
            return Ok(notificationdata);
        }

        [HttpGet("details/{Id}")]
        public async Task<ActionResult<NotificationResource>> GetdistributorIdById(string Id)
        {
            var notification = await _NotificationDistributorService.GetNotificationByIdAsync(Id);
            var distributorlist = await _NotificationDistributorService.GetDistableByNotificationIdAsync(Id);

            var drsdetaildata = _mapper.Map<List<NotificationDistributor>, List<NotificationDistributorResource>>((List<NotificationDistributor>)distributorlist);
            var notificationdata = _mapper.Map<Notification, NotificationResource>(notification);
            notificationdata.distributorlist = drsdetaildata;
            return Ok(notificationdata);
        }

        [HttpGet("Executive/{Id}")]
        public async Task<ActionResult> GetexecutiveIdById(string Id)
        {
          var executiveId = await _NotificationDistributorService.executiveid(Id);
            return Ok(executiveId);
        }


        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateNotificationc([FromBody] NotificationResource notification)
        {
            try
            {
                var notifydata = _mapper.Map<NotificationResource, Notification>(notification);
                var Distributorlist = notification.distributorlist;
                var drsdetaildata = _mapper.Map<List<NotificationDistributorResource>, List<NotificationDistributor>>(Distributorlist);

                var ExecutiveList = notification.executiveList; 

                var executiveData = _mapper.Map<List<NotificationExecutiveResource>, List<NotificationExecutive>>(ExecutiveList);

                var creadted = await _NotificationDistributorService.CreateNotificationc(notifydata, drsdetaildata, executiveData);

                return StatusCode(creadted.StatusCode, creadted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }




        [HttpDelete("{Id}")]
        public async Task<ActionResult<DsrResponse>> DeleteNotification(String Id)
        {
           
            var result = await _NotificationDistributorService.DeleteNotification(Id);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> EditResponse(string id, [FromBody] NotificationResource notification)
        {

            var notifydata = _mapper.Map<NotificationResource, Notification>(notification);
            var Distributorlist = notification.distributorlist;
            var drsdetaildata = _mapper.Map<List<NotificationDistributorResource>, List<NotificationDistributor>>(Distributorlist);

            var result = await _NotificationDistributorService.updateNotificationc(notifydata, drsdetaildata);
            return Ok(result);

        }



        [HttpPost("Search")]

        public async Task<ActionResult<IEnumerable<NotificationResource>>> SearchDsr([FromBody] DsrDetailsByIdResource search)

        {
          
            var mapped = _mapper.Map<DsrDetailsByIdResource, EdittDSR>(search);
            var selut = await _NotificationDistributorService.SearchNotification(mapped);

            var notifydata = _mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationResource>>(selut);
            return Ok(notifydata);
        }

    }
}

