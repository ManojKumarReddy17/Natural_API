using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using Natural_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable


namespace Natural_Data.Repositories
{
    public class AssignDistributorToExecutiveRepository : Repository<DistributorToExecutive>, IAssignDistributorToExecutiveRepository
    {
        public AssignDistributorToExecutiveRepository(NaturalsContext context) : base(context)
        {

        }

        public async Task<IEnumerable<DistributorToExecutive>> GetAssignedDistributorByExecutiveIdAsync(string ExecutiveId)
        {
            return await NaturalDbContext.DistributorToExecutives.
             Where(c => c.ExecutiveId == ExecutiveId && c.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<Distributor>> GetAssignedDistributorDetailsByExecutiveIdAsync(string ExecutiveId)
        {
            var AssignedList = await NaturalDbContext.DistributorToExecutives
                .Include(D => D.Distributor)
                .ThenInclude(d => d.AreaNavigation)  
                .ThenInclude(a => a.City)    
                .ThenInclude(c => c.State)           
                .Include(D => D.Executive)
                .Where(c => c.ExecutiveId == ExecutiveId && c.IsDeleted == false)
                .ToListAsync();

            var result = AssignedList.Select(c => new Distributor
            {
                Id = c.Distributor.Id,
                FirstName = c.Distributor.FirstName,
                LastName = c.Distributor.LastName,
                MobileNumber = c.Distributor.MobileNumber,
                Address = c.Distributor.Address,
                Email = c.Distributor.Email,
                Area = c.Distributor.AreaNavigation.AreaName,
                City = c.Distributor.AreaNavigation.City.CityName,
                State = c.Distributor.AreaNavigation.City.State?.StateName
            }).ToList();
            return result;
        }
        public async Task<bool> IsDistirbutorAssignedToExecutiveAsync(List<string> distributorIds)

        {
            var existingAssignment = await NaturalDbContext.DistributorToExecutives
                .AnyAsync(d => distributorIds.Contains(d.DistributorId));

            return existingAssignment;
        }

        public async Task<DistributorToExecutive> DeleteDistributorAsync(string distributorId, string ExecutiveId)
        {
            var result = await NaturalDbContext.DistributorToExecutives
                .Where(d => d.DistributorId == distributorId && d.ExecutiveId == ExecutiveId )
                .FirstOrDefaultAsync();

            return result;
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
