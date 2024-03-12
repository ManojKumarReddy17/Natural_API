using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Natural_Core.Models;
using Natural_Core.S3Models;

namespace Natural_Core.IServices
{
	public interface INotificationDistributorService
	{

        Task<IEnumerable<Notification>> GetAll();
        Task<ProductResponse> CreateNotificationc(Notification notification, List<NotificationDistributor> distributors);
        Task<IEnumerable<NotificationDistributor>> GetDistributorsByNotificationIdAsync(string notificationId);
        Task<IEnumerable<NotificationDistributor>> GetDistableByNotificationIdAsync(string notificationId);
        Task<Notification> GetNotificationByIdAsync(string Id);
        Task<DsrResponse> DeleteNotification(string id);
        Task<string> executiveid(string distributorId);
        Task<ProductResponse> updateNotificationc(Notification notification, List<NotificationDistributor> distributors);
        Task<IEnumerable<Notification>> SearchNotification(EdittDSR search);


    }
}

