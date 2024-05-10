using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#nullable disable

namespace Natural_Data.Repositories
{
    public class AssignRetailorToDistributorRepository : Repository<RetailorToDistributor>, IAssignRetailorToDistributorRepository
    {
        public AssignRetailorToDistributorRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<RetailorToDistributor>> GetAssignedRetailorsIdByDistributorIdAsync(string distributorId)
        {
            var AssignedList = await NaturalDbContext.RetailorToDistributors.Where(c=>c.DistributorId == distributorId && c.IsDeleted == false).ToListAsync();
            return AssignedList;       

        }
        public async Task<IEnumerable<Retailor>> GetAssignedRetailorDetailsByDistributorIdAsync(string distributorId)
        {
            var AssignedList = await NaturalDbContext.RetailorToDistributors
                .Include(D => D.Retailor)
                .ThenInclude(d => d.AreaNavigation)
                .ThenInclude(a => a.City)
                .ThenInclude(c => c.State)
                .Include(D => D.Distributor)
                .Where(rt => rt.DistributorId == distributorId  && rt.IsDeleted == false)
                .ToListAsync();

            var result = AssignedList.Select(c => new Retailor
            {
                Id = c.Retailor.Id,
                FirstName = c.Retailor.FirstName,
                Address = c.Retailor.Address,
                LastName = c.Retailor.LastName,
                MobileNumber = c.Retailor.MobileNumber,
                Email = c.Retailor.Email,
                Area = c.Retailor.AreaNavigation.AreaName,
                City = c.Retailor.AreaNavigation.City.CityName,
                State = c.Retailor.StateNavigation.StateName,
                Image = c.Retailor.Image,

            }).ToList();

            return result;
        }



    

        public async Task<bool> IsRetailorAssignedToDistirbutor(List<string> retailorid)
        {
            var existingAssignment = await NaturalDbContext.RetailorToDistributors
                           .AnyAsync(d => retailorid.Contains(d.RetailorId) );

            return existingAssignment;
        }


        public async Task<RetailorToDistributor> DeleteRetailorAsync(string RetailorID, string DistributorId)
        {
            var result = await NaturalDbContext.RetailorToDistributors
                .Where(d => d.DistributorId == RetailorID && d.DistributorId == DistributorId )
                .FirstOrDefaultAsync();

            return result;
        }
        private NaturalsContext  NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }


}
