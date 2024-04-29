
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Data.Repositories
{
    public class DsrRepository : Repository<Dsr>, IDsrRepository
    {

        public DsrRepository(NaturalsContext context) : base(context)
        {


        }


        

        public async Task<IEnumerable<Dsr>> GetAllDsrAsync()
        {
            var dsr = await NaturalDbContext.Dsrs
                   .Include(c => c.ExecutiveNavigation)
                   .Include(c => c.DistributorNavigation)
                   .Include(c => c.RetailorNavigation)
                   .Include(c => c.OrderByNavigation)
                   .Where(c => c.IsDeleted != true)
                   .Select(c => new Dsr
                   {
                       Id = c.Id,
                       Executive = string.Concat(c.ExecutiveNavigation.FirstName, "", c.ExecutiveNavigation.LastName),
                       Distributor = string.Concat(c.DistributorNavigation.FirstName, "", c.DistributorNavigation.LastName),
                       Retailor = string.Concat(c.RetailorNavigation.FirstName, "", c.RetailorNavigation.LastName),
                       OrderBy = string.Concat(c.OrderByNavigation.FirstName, "", c.OrderByNavigation.LastName),
                       CreatedDate = c.CreatedDate,
                       ModifiedDate = c.ModifiedDate,
                       TotalAmount = c.TotalAmount,
                      

                   }).ToListAsync();
            return dsr;

        }

       


        public async Task<IEnumerable<Dsr>> SearchDsr(EdittDSR search)
        {
            var dsr = NaturalDbContext.Dsrs
                 .Include(c => c.ExecutiveNavigation)
                 .Include(c => c.DistributorNavigation)
                 .Include(c => c.RetailorNavigation)
                  .Include(c => c.OrderByNavigation)
                      .Where(c =>
                    (string.IsNullOrEmpty(search.Executive) || c.Executive == search.Executive) &&
                    (string.IsNullOrEmpty(search.Distributor) || c.Distributor == search.Distributor) &&
                    (string.IsNullOrEmpty(search.Retailor) || c.Retailor == search.Retailor) &&
                    (string.IsNullOrEmpty(search.OrderBy) || c.OrderBy == search.OrderBy)
                    &&

                      (search.StartDate == null || c.CreatedDate.Date >= search.StartDate.Date && c.CreatedDate.Date <= search.EndDate.Date))
                     .Where (c=> c.IsDeleted != true)

                .Select(c => new Dsr
                {
                    Id = c.Id,
                    Executive = string.Concat(c.ExecutiveNavigation.FirstName, c.ExecutiveNavigation.LastName),
                    Distributor = string.Concat(c.DistributorNavigation.FirstName, c.DistributorNavigation.LastName),
                    Retailor = string.Concat(c.RetailorNavigation.FirstName, c.RetailorNavigation.LastName),
                    TotalAmount = c.TotalAmount,
                    OrderBy = string.Concat(c.OrderByNavigation.FirstName, c.OrderByNavigation.LastName),
                    CreatedDate = c.CreatedDate

                })
                .ToList();
            return dsr;

        }




        public async Task<IEnumerable<Product>> GetProductDetailsByDsrIdAsync(string dsrId)
        {
            var productDetails = await NaturalDbContext.Dsrdetails
                .Where(d => d.Dsr == dsrId)
                .Select(d => new Product
                {
                    Id = d.ProductNavigation.Id,                
                    ProductName = d.ProductNavigation.ProductName,
                    Price = d.ProductNavigation.Price,
                   
                    Quantity = d.Quantity,
                    Weight = d.ProductNavigation.Weight
                })
                .ToListAsync();

            return productDetails;
        }


        public async Task<IEnumerable<DsrDistributor>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {

            var AssignedList = await NaturalDbContext.DistributorToExecutives.
                Include(D => D.Distributor).
                Include(D => D.Executive).Where(c => c.ExecutiveId == ExecutiveId && c.IsDeleted != true).ToListAsync();
            var result = AssignedList.Select(c => new DsrDistributor
            {
                Id = c.Distributor.Id,

                DistributorName = string.Concat(c.Distributor.FirstName, "", c.Distributor.LastName)
            }).ToList();

            return result;

        }

        public async Task<IEnumerable<DsrRetailor>> GetAssignedRetailorDetailsByDistributorId(string DistributorId)
        {

            var AssignedList = await NaturalDbContext.RetailorToDistributors.
                Include(D => D.Retailor).
                Include(D => D.Distributor).Where(c => c.DistributorId == DistributorId && c.IsDeleted != true).ToListAsync();
            var result = AssignedList.Select(c => new DsrRetailor
            {
                Id = c.Retailor.Id,
                Retailor = string.Concat(c.Retailor.FirstName, "", c.Retailor.LastName)
            }).ToList();

            return result;

        }

        public async Task<Dsr> GetDsrbyId(string dsrid)
        {
            var dsr =await NaturalDbContext.Dsrs
                     .Include(c => c.ExecutiveNavigation)
                     .Include(c => c.DistributorNavigation)
                     .Include(c => c.RetailorNavigation)
                     .Include(c => c.OrderByNavigation)
                     .Where(c => c.Id == dsrid && c.IsDeleted != true)
                     .Select(c => new Dsr
                     {
                         Id = c.Id,
                         Executive = string.Concat(c.ExecutiveNavigation.FirstName,"", c.ExecutiveNavigation.LastName),
                         Distributor = string.Concat(c.DistributorNavigation.FirstName,"", c.DistributorNavigation.LastName),
                         Retailor = string.Concat(c.RetailorNavigation.FirstName,"", c.RetailorNavigation.LastName),
                         OrderBy = string.Concat(c.OrderByNavigation.FirstName,"", c.OrderByNavigation.LastName),
                         CreatedDate = c.CreatedDate,
                         ModifiedDate = c.ModifiedDate,
                         TotalAmount = c.TotalAmount

                     }).FirstOrDefaultAsync();

            return dsr;
        }
        public async Task<IEnumerable<Dsr>> GetRetailorDetailsByDistributorId(string distributorId)
        {
            var dsr = await NaturalDbContext.Dsrs
                .Include(c => c.ExecutiveNavigation)
                .Include(c => c.DistributorNavigation)
                .Include(c => c.RetailorNavigation)
                .Include(c => c.OrderByNavigation)
                .Where(c => c.DistributorNavigation.Id == distributorId && c.IsDeleted != true)
                .Select(c => new Dsr
                {
                    Id = c.Id,
                    Executive = string.Concat(c.ExecutiveNavigation.FirstName, "", c.ExecutiveNavigation.LastName),
                    Distributor = string.Concat(c.DistributorNavigation.FirstName, "", c.DistributorNavigation.LastName),
                    Retailor = string.Concat(c.RetailorNavigation.FirstName, "", c.RetailorNavigation.LastName),
                    OrderBy = string.Concat(c.OrderByNavigation.FirstName, "", c.OrderByNavigation.LastName),
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate,
                    TotalAmount = c.TotalAmount,
                   
                }).ToListAsync();

            return dsr;
        }





        public async Task<IEnumerable<Dsr>> GetRetailorDetailsByDate(string distributorId, DateTime date)
        {
            var retailorList = await NaturalDbContext.Dsrs
                .Include(c => c.ExecutiveNavigation)
                .Include(c => c.DistributorNavigation)
                .Include(c => c.RetailorNavigation)
                .Include(c => c.OrderByNavigation)
                .Where(c => c.Distributor == distributorId && c.IsDeleted != true)
                .Select(c => new Dsr
                {
                    Id = c.Id,
                    Executive = string.Concat(c.ExecutiveNavigation.FirstName, "", c.ExecutiveNavigation.LastName),
                    Distributor = string.Concat(c.DistributorNavigation.FirstName, "", c.DistributorNavigation.LastName),
                    Retailor = string.Concat(c.RetailorNavigation.FirstName, "", c.RetailorNavigation.LastName),
                    OrderBy = string.Concat(c.OrderByNavigation.FirstName, "", c.OrderByNavigation.LastName),
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate,
                    TotalAmount = c.TotalAmount
                })
                .ToListAsync();
            var retailor = retailorList.Where(c => c.CreatedDate.Date == date.Date).ToList();

            return retailor;
        }

        public async Task<IEnumerable<Dsr>> GetRetailorDetailsByExecutiveId(string executiveId)
        {
            var dsr = await NaturalDbContext.Dsrs
                   .Include(c => c.ExecutiveNavigation)
                   .Include(c => c.DistributorNavigation)
                   .Include(c => c.RetailorNavigation)
                   .Include(c => c.OrderByNavigation)
                   .Where(c => c.ExecutiveNavigation.Id == executiveId && c.IsDeleted != true)
                   .Select(c => new Dsr
                   {
                       Id = c.Id,
                       Executive = string.Concat(c.ExecutiveNavigation.FirstName, "", c.ExecutiveNavigation.LastName),
                       Distributor = string.Concat(c.DistributorNavigation.FirstName, "", c.DistributorNavigation.LastName),
                       Retailor = string.Concat(c.RetailorNavigation.FirstName, "", c.RetailorNavigation.LastName),
                       OrderBy = string.Concat(c.OrderByNavigation.FirstName, "", c.OrderByNavigation.LastName),
                       CreatedDate = c.CreatedDate,
                       ModifiedDate = c.ModifiedDate,
                       TotalAmount = c.TotalAmount

                   }).ToListAsync();
            return dsr;


        }




    



        public async Task<IEnumerable<Dsr>> SearchDsrByDistributorIds(string distributorId)
        {
            var dsrs = await NaturalDbContext.Dsrs
                .Include(c => c.ExecutiveNavigation)
                .Include(c => c.DistributorNavigation)
                .Include(c => c.RetailorNavigation)
                .Include(c => c.OrderByNavigation)
               .Where(c => c.Distributor == distributorId )
                .Select(c => new Dsr
                {
                    Id = c.Id,
                    Executive = string.Concat(c.ExecutiveNavigation.FirstName, "", c.ExecutiveNavigation.LastName),
                    Distributor = string.Concat(c.DistributorNavigation.FirstName, "", c.DistributorNavigation.LastName),
                    Retailor = string.Concat(c.RetailorNavigation.FirstName, "", c.RetailorNavigation.LastName),
                    OrderBy = string.Concat(c.OrderByNavigation.FirstName, "", c.OrderByNavigation.LastName),
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate,
                    TotalAmount = c.TotalAmount
                }).ToListAsync();
            return dsrs;
        }

        public async Task<IEnumerable<DSRretailorDetails>> GetRetailorDetails(string ExecutiveId)
        {

            var dsr = await NaturalDbContext.Dsrs
                   .Include(c => c.ExecutiveNavigation)
                   .Include(c => c.DistributorNavigation)
                   .Include(c => c.RetailorNavigation)
                   .ThenInclude(a => a.AreaNavigation)
                  
                   .Where(d => d.Executive == ExecutiveId && d.IsDeleted != true)

                   .Select(c => new DSRretailorDetails
                   {
                       Id = c.Id,
                       Executive = string.Concat(c.ExecutiveNavigation.FirstName, "", c.ExecutiveNavigation.LastName),
                       Distributor = string.Concat(c.DistributorNavigation.FirstName, "", c.DistributorNavigation.LastName),
                       Retailor = string.Concat(c.RetailorNavigation.FirstName, "", c.RetailorNavigation.LastName),
                       Phonenumber = c.RetailorNavigation.MobileNumber,
                       Address = c.RetailorNavigation.Address,
                       City = c.RetailorNavigation.CityNavigation.CityName,
                       State = c.RetailorNavigation.StateNavigation.StateName,
                       OrderBy = c.OrderBy,
                       TotalAmount = c.TotalAmount,
                       CreatedDate = c.RetailorNavigation.CreatedDate,
                       ModifiedDate = c.RetailorNavigation.ModifiedDate,
                       Area = c.RetailorNavigation.AreaNavigation.AreaName,
                       Image = c.RetailorNavigation.Image

                      

                   }).ToListAsync();

            return dsr;

        }

        private NaturalsContext NaturalDbContext
        {
            get
            {
                return Context as NaturalsContext;
            }
        }
    }
}
