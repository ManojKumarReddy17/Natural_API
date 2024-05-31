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

            var distributors = await NaturalDbContext.NotificationDistributors
                   .Include(c => c.DistributorNavigation)
                   .Where(c => c.Notification == notificationId && c.IsDeleted == false)
                   .Select(c => new NotificationDistributor
                   {
                       Distributor = c.DistributorNavigation.FirstName + " " + c.DistributorNavigation.LastName,
                       Id = c.Id
                   }).ToListAsync();
            return distributors;

        }


        public async Task<IEnumerable<NotificationDistributor>> GetDisTableByNotificationIdAsync(string notificationId)
        {
            var distributorslist = await NaturalDbContext.NotificationDistributors.Where(x => x.Notification == notificationId && x.IsDeleted == false).ToListAsync();
            return distributorslist;

        }


        public async Task<string> executiveid(string notificationId)
        {
            var executiveId = await NaturalDbContext.DistributorToExecutives.Where(x => x.DistributorId == notificationId && x.IsDeleted != true).Select(x => x.ExecutiveId)
            .FirstOrDefaultAsync();
            return executiveId;

        }
        public async Task<IEnumerable<DistributorNotificationDetails>> GetNotificationsbyDistrbId(string DistributorId)
        {
            var Distributor = await NaturalDbContext.NotificationDistributors
                .Include(c => c.NotificationNavigation)
                .Where(c => c.Distributor == DistributorId).Select(c =>
                new DistributorNotificationDetails
                {
                    Id = c.NotificationNavigation.Id,
                    Body = c.NotificationNavigation.Body,
                    Subject = c.NotificationNavigation.Subject,

                }).ToListAsync();
            return Distributor;

        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}

