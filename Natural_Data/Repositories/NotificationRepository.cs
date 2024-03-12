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
            && x.CreatedDate.Date <= search.EndDate.Date).Select(x => new Notification

            {
                Id =x.Id,
                Body = x.Body,
                Subject = x.Subject,
                CreatedDate = x.CreatedDate
            }).ToListAsync();

            return searchresult;

        }


        //public Task AddAsync(Notification entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task AddRangeAsync(IEnumerable<Notification> entities)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Notification> Find(Expression<Func<Notification, bool>> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<Notification>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public ValueTask<Notification> GetByIdAsync(string id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Remove(Notification entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveRange(IEnumerable<Notification> entities)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Notification> SingleOrDefaultAsync(Expression<Func<Notification, bool>> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(Notification entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateRange(IEnumerable<Notification> entities)
        //{
        //    throw new NotImplementedException();
        //}


        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}

