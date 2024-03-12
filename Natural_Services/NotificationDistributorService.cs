using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3Models;

namespace Natural_Services
{
	public class NotificationDistributorService: INotificationDistributorService
    {
		

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public NotificationDistributorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Notification>> GetAll()
        {

           var notification = await _unitOfWork.NotificationRepository.GetAllAsync();

            return notification;
        }

        public async Task<Notification> GetNotificationByIdAsync(string Id)
        {
        var notification =  await   _unitOfWork.NotificationRepository.GetByIdAsync(Id);
            return notification;
        }


        public async Task<IEnumerable<NotificationDistributor>> GetDistributorsByNotificationIdAsync(string notificationId)
        {


          var distributorslist =   await _unitOfWork.NotificationDistributorRepository.GetDistributorByNotificationIdAsync(notificationId);

            return distributorslist;

        }

       public async Task<IEnumerable<NotificationDistributor>> GetDistableByNotificationIdAsync(string notificationId)
        {
           var distributorslist = await _unitOfWork.NotificationDistributorRepository.GetDisTableByNotificationIdAsync(notificationId);
            return distributorslist;

        }

        public async Task<ProductResponse> CreateNotificationc(Notification notification, List<NotificationDistributor> distributors)
        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {
                    notification.Id = "Noti" + new Random().Next(10000, 99999).ToString();


                    await _unitOfWork.NotificationRepository.AddAsync(notification);
                    var commit = await _unitOfWork.CommitAsync();

                    var create = distributors.Select(c => new NotificationDistributor
                    {
                        
                        Distributor  = c.Distributor,
                        Notification = notification.Id

                    }).ToList();

                    await _unitOfWork.NotificationDistributorRepository.AddRangeAsync(create);
                    var commit1 = await _unitOfWork.CommitAsync();

                    transaction.Commit();

                    response.Message = " Notification and Distributordetail Insertion Successful";
                    response.StatusCode = 200;
                    response.Id = notification.Id;


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Insertion Failed";
                    response.StatusCode = 401;

                }

                return response;
            }

        }

        public async Task<DsrResponse> DeleteNotification(string id)
        {

          var notification= await GetNotificationByIdAsync(id);
            var distributionlist = await GetDistableByNotificationIdAsync(id);

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new DsrResponse();

                try
                {

                    _unitOfWork.NotificationDistributorRepository.RemoveRange(distributionlist);
                    var commit = await _unitOfWork.CommitAsync();
                    _unitOfWork.NotificationRepository.Remove(notification);
                    var commit1 = await _unitOfWork.CommitAsync();

                    transaction.Commit();
                    response.Message = "SUCCESSFULLY DELETED";
                    response.StatusCode = 200;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "delete Failed";
                    response.StatusCode = 401;
                }

                return response;
            }


        }

        public async Task<string> executiveid(string distributorId)
        {
          var executiveId =   await _unitOfWork.NotificationDistributorRepository.executiveid(distributorId);
            return executiveId;
        }

        public async Task<IEnumerable<Notification>> SearchNotification(EdittDSR search)
        {
          var searchresult =  await _unitOfWork.NotificationRepository.SearchNotification(search);
            return searchresult;
        }

        public async Task<ProductResponse> updateNotificationc(Notification notification, List<NotificationDistributor> distributors)
        {



            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {

                    var existingnotification = await GetNotificationByIdAsync(notification.Id);

                    existingnotification.Subject = notification.Subject;
                    existingnotification.Body = notification.Body;

                    _unitOfWork.NotificationRepository.Update(existingnotification);


                    var commit = await _unitOfWork.CommitAsync();

                 

                    var existingdist = await GetDistableByNotificationIdAsync(notification.Id);
                    var result = _mapper.Map<List<NotificationDistributor>>(existingdist);


                    var differentRecords = distributors.Except(result, new notificationComparer()).ToList();

                    await _unitOfWork.NotificationDistributorRepository.AddRangeAsync(differentRecords);

                    var created = await _unitOfWork.CommitAsync();

                    var deletingRecords =  result.Except(distributors ,new notificationComparer()).ToList();


                    _unitOfWork.NotificationDistributorRepository.RemoveRange(deletingRecords);
                    var deted = await _unitOfWork.CommitAsync();

                    


                    transaction.Commit();

                    response.Message = " Dsr and DsrdetailInsertion Successful";
                    response.StatusCode = 200;
                    response.Id = notification.Id;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Insertion Failed";
                    response.StatusCode = 401;

                }

                return response;
            }


        }

    }




    class notificationComparer : IEqualityComparer<NotificationDistributor>
    {
        public bool Equals(NotificationDistributor x, NotificationDistributor y)
        {
            if (x.Distributor== y.Distributor)
                return true;

            return false;
        }

        public int GetHashCode(NotificationDistributor obj)
        {
            return obj.Distributor.GetHashCode();
        }

       
    }
}

