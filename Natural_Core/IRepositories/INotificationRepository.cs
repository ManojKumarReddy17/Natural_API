using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Natural_Core.Models;

namespace Natural_Core.IRepositories
{
	public interface INotificationRepository: IRepository<Notification>
    {

        Task<IEnumerable<Notification>> SearchNotification(EdittDSR search);

    }
}

