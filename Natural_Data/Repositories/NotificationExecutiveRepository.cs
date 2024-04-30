using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Data.Repositories
{
    public class NotificationExecutiveRepository : Repository<NotificationExecutive>, INotificationExecutiveRepository
    {
        public NotificationExecutiveRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<NotificationExecutive>> GetExecutiveByNotificationIdAsync(string notiId)
        {

            var executives = await NaturalDbContext.NotificationExecutives
                   .Include(c => c.ExecutiveNavigation)
                   .Where(c => c.Notification == notiId && c.IsDeleted == false)
                   .Select(c => new NotificationExecutive
                   {
                       Executive = c.ExecutiveNavigation.FirstName + " " + c.ExecutiveNavigation.LastName,
                       Id = c.Id
                   }).ToListAsync();
            return executives;

        }

        public async Task<IEnumerable<NotificationExecutive>> GetexeTableByNotificationIdAsync(string notiId)
        {
            var executiveslist = await NaturalDbContext.NotificationExecutives.Where(x => x.Notification == notiId && x.IsDeleted == false).ToListAsync();
            return executiveslist;

        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
