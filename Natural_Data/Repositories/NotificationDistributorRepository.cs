using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;

namespace Natural_Data.Repositories
{
	public class NotificationDistributorRepository : Repository<NotificationDistributor>, INotificationDistributorRepository
    {
        public NotificationDistributorRepository(NaturalsContext context) : base(context)
        {

        }

       public async Task<IEnumerable<NotificationDistributor>> GetDistributorByNotificationIdAsync(string notificationId)
        {

         var distributors =    await NaturalDbContext.NotificationDistributors
                .Include(c => c.DistributorNavigation)
                .Where(c => c.Notification == notificationId)
                .Select(c => new NotificationDistributor
                {
                    Distributor = c.DistributorNavigation.FirstName + " " + c.DistributorNavigation.LastName,
                    Id = c.Id
                }).ToListAsync();
            return distributors;

        }


        public async Task<IEnumerable<NotificationDistributor>> GetDisTableByNotificationIdAsync(string notificationId)
        {
           var distributorslist = await NaturalDbContext.NotificationDistributors.Where(x => x.Notification == notificationId).ToListAsync();
            return distributorslist;

        }


        public async Task<string> executiveid(string notificationId)
        {
            var executiveId = await NaturalDbContext.DistributorToExecutives.Where(x => x.DistributorId == notificationId).Select(x => x.ExecutiveId)
            .FirstOrDefaultAsync();
            return executiveId;

        }


        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}

