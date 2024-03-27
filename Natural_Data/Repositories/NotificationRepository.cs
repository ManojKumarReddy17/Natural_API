using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;

namespace Natural_Data.Repositories
{
	public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
		
        public NotificationRepository(NaturalsContext context) : base(context)
        {

        }


       public async Task<IEnumerable<Notification>> SearchNotification(EdittDSR search)
        {

           var searchresult = await NaturalDbContext.Notifications
                .Where(x => search.StartDate == null
            || x.CreatedDate.Date >= search.StartDate.Date
            && x.CreatedDate.Date <= search.EndDate.Date && x.IsDeleted ==false).Select(x => new Notification

            {
                Id =x.Id,
                Body = x.Body,
                Subject = x.Subject,
                CreatedDate = x.CreatedDate
            }).ToListAsync();

            return searchresult;

        }


       


        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}

