using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Natural_Core.Models;

namespace Natural_Core.IRepositories
{
	public interface INotificationDistributorRepository: IRepository<NotificationDistributor>
    {
        Task<IEnumerable<NotificationDistributor>> GetDistributorByNotificationIdAsync(string notificationId);
        Task<IEnumerable<NotificationDistributor>> GetDisTableByNotificationIdAsync(string notificationId);
        Task<string> executiveid(string distributorId);
        Task<IEnumerable<DistributorNotificationDetails>> GetNotificationsbyDistrbId(string DistributorId);
    }
}

