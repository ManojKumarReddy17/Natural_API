using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface INotificationExecutiveRepository : IRepository<NotificationExecutive>
    {
        Task<IEnumerable<NotificationExecutive>> GetExecutiveByNotificationIdAsync(string notiId);
        Task<IEnumerable<NotificationExecutive>> GetexeTableByNotificationIdAsync(string notiId);
    }
}
