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
    public class NotificationService : INotificationService
    {


        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Notification>> GetAll()
        {

            var notification = await _unitOfWork.NotificationRepository.GetAllAsync();
            var PresentNotif = notification.Where(c => c.IsDeleted == false).ToList();
            return PresentNotif;
        }

        public async Task<Notification> GetNotificationByIdAsync(string Id)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(Id);
            //   var PresentNotif = notification.Where(c => c.IsDeleted == false).ToList();
            if (notification.IsDeleted == false)
            {
                return notification;
            }
            return null;
        }
        public async Task<IEnumerable<ExecutiveNotificationDetails>> GetNotificationsbyexecid(string ExecutiveId)
        {
            var executiveres = await _unitOfWork.NotificationExecutiveRepository.GetNotificationsbyexecid(ExecutiveId);
            return executiveres;
        }
        public async Task<IEnumerable<DistributorNotificationDetails>> GetNotificationsbyDistrbId(string DistributorId)
        {
            var Distributors = await _unitOfWork.NotificationDistributorRepository.GetNotificationsbyDistrbId(DistributorId);
            return Distributors;
        }



        public async Task<IEnumerable<NotificationDistributor>> GetDistributorsByNotificationIdAsync(string notificationId)
        {


            var distributorslist = await _unitOfWork.NotificationDistributorRepository.GetDistributorByNotificationIdAsync(notificationId);
            //var PresentDist = distributorslist.Where(c => c.IsDeleted != true).ToList();
            return distributorslist;

        }

        public async Task<IEnumerable<NotificationExecutive>> GetExecutivesByNotificationIdAsync(string notiId)
        {
            var executiveslist = await _unitOfWork.NotificationExecutiveRepository.GetExecutiveByNotificationIdAsync(notiId);
            return executiveslist;
        }

        public async Task<IEnumerable<NotificationDistributor>> GetDistableByNotificationIdAsync(string notificationId)
        {
            var distributorslist = await _unitOfWork.NotificationDistributorRepository.GetDisTableByNotificationIdAsync(notificationId);
            // var PresentDist = distributorslist.Where(c => c.IsDeleted!= true).ToList();
            return distributorslist;

        }
        public async Task<IEnumerable<NotificationExecutive>> GetExetableByNotificationIdAsync(string notiId)
        {
            var exelist = await _unitOfWork.NotificationExecutiveRepository.GetexeTableByNotificationIdAsync(notiId);
            return exelist;

        }


        public async Task<ProductResponse> CreateNotificationc(Notification notification, List<NotificationDistributor> distributors, List<NotificationExecutive> executives)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {
                    notification.Id = "Noti" + new Random().Next(10000, 99999).ToString();
                    await _unitOfWork.NotificationRepository.AddAsync(notification);
                    var commitNotification = await _unitOfWork.CommitAsync();

                    var createDistributors = distributors.Select(c => new NotificationDistributor
                    {
                        Distributor = c.Distributor,
                        Notification = notification.Id
                    }).ToList();
                    await _unitOfWork.NotificationDistributorRepository.AddRangeAsync(createDistributors);
                    var commitDistributors = await _unitOfWork.CommitAsync();

                    var createExecutives = executives.Select(e => new NotificationExecutive
                    {
                        Executive = e.Executive,
                        Notification = notification.Id
                    }).ToList();
                    await _unitOfWork.NotificationExecutiveRepository.AddRangeAsync(createExecutives);
                    var commitExecutives = await _unitOfWork.CommitAsync();

                    transaction.Commit();

                    response.Message = "Notification, Distributor, and Executive Insertion Successful";
                    response.StatusCode = 200;
                    response.Id = notification.Id;
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    transaction.Rollback();
                    response.Message = "Insertion Failed: " + ex.Message;
                    response.StatusCode = 401; // Or any appropriate error code
                }

                return response;
            }
        }

        //public async Task<DsrResponse> DeleteNotification(string id)
        //{

        //    var notification = await GetNotificationByIdAsync(id);
        //    var distributionlist = await GetDistableByNotificationIdAsync(id);

        //    using (var transaction = _unitOfWork.BeginTransaction())
        //    {
        //        var response = new DsrResponse();

        //        try
        //        {
        //            foreach (var distribution in distributionlist)
        //            {
        //                distribution.IsDeleted = true;
        //                _unitOfWork.NotificationDistributorRepository.Update(distribution);
        //            }
        //            notification.IsDeleted = true;
        //            _unitOfWork.NotificationRepository.Update(notification);
        //            var commit1 = await _unitOfWork.CommitAsync();

        //            transaction.Commit();
        //            response.Message = "SUCCESSFULLY DELETED";
        //            response.StatusCode = 200;

        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            response.Message = "delete Failed";
        //            response.StatusCode = 401;
        //        }

        //        return response;
        //    }


        //}

        public async Task<DsrResponse> DeleteNotification(string id)
        {

            var notification = await GetNotificationByIdAsync(id);
            var distributionlist = await GetDistableByNotificationIdAsync(id);
            var Exelist = await GetExetableByNotificationIdAsync(id);

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new DsrResponse();

                try
                {
                    foreach (var distribution in distributionlist)
                    {
                        distribution.IsDeleted = true;
                        _unitOfWork.NotificationDistributorRepository.Update(distribution);
                    }
                    foreach (var execu in Exelist)
                    {
                        execu.IsDeleted = true;
                        _unitOfWork.NotificationExecutiveRepository.Update(execu);
                    }
                    notification.IsDeleted = true;
                    _unitOfWork.NotificationRepository.Update(notification);
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
            var executiveId = await _unitOfWork.NotificationDistributorRepository.executiveid(distributorId);

            return executiveId;
        }

        public async Task<IEnumerable<Notification>> SearchNotification(EdittDSR search)
        {
            var searchresult = await _unitOfWork.NotificationRepository.SearchNotification(search);
            return searchresult;
        }

        public async Task<ProductResponse> updateNotificationc(Notification notification, List<NotificationDistributor> distributors, List<NotificationExecutive> executives)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {
                    var existingNotification = await GetNotificationByIdAsync(notification.Id);
                    existingNotification.Subject = notification.Subject;
                    existingNotification.Body = notification.Body;
                    _unitOfWork.NotificationRepository.Update(existingNotification);
                    await _unitOfWork.CommitAsync();

                    var existingDistributors = await GetDistributorsByNotificationIdAsync(notification.Id);
                    var existingExecutives = await GetExecutivesByNotificationIdAsync(notification.Id);

                    var mappedDistributors = _mapper.Map<List<NotificationDistributor>>(existingDistributors);
                    var mappedExecutives = _mapper.Map<List<NotificationExecutive>>(existingExecutives);

                    var differentDistributors = distributors.Except(mappedDistributors, new notificationComparer()).ToList();
                    await _unitOfWork.NotificationDistributorRepository.AddRangeAsync(differentDistributors);

                    var differentExecutives = executives.Except(mappedExecutives, new notificationComparer()).ToList(); // Assuming executive is a single entity
                    await _unitOfWork.NotificationExecutiveRepository.AddRangeAsync(differentExecutives);

                    var created = await _unitOfWork.CommitAsync();

                    var deletingDistributors = mappedDistributors.Except(distributors, new notificationComparer()).ToList();
                    _unitOfWork.NotificationDistributorRepository.RemoveRange(deletingDistributors);

                    var deletingExecutives = mappedExecutives.Except(executives, new notificationComparer()).ToList();
                    _unitOfWork.NotificationExecutiveRepository.RemoveRange(deletingExecutives);
                    var deted = await _unitOfWork.CommitAsync();
                    //await _unitOfWork.CommitAsync();

                    transaction.Commit();

                    response.Message = "Distributor, Executive, and Notification updated successfully";
                    response.StatusCode = 200;
                    response.Id = notification.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Update failed";
                    response.StatusCode = 401;
                }
                return response;
            }
        }


    }




    class notificationComparer : IEqualityComparer<NotificationDistributor>, IEqualityComparer<NotificationExecutive>
    {
        public bool Equals(NotificationDistributor x, NotificationDistributor y)
        {
            if (x.Distributor == y.Distributor)
                return true;

            return false;
        }
        public bool Equals(NotificationExecutive x, NotificationExecutive y)
        {
            if (x.Executive == y.Executive)
                return true;

            return false;
        }

        public int GetHashCode(NotificationDistributor obj)
        {
            return obj.Distributor.GetHashCode();
        }
        public int GetHashCode(NotificationExecutive obj)
        {
            return obj.Executive.GetHashCode();
        }

    }
}