using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using Natural_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public async Task<IEnumerable<Dsr>> SearchDsr(Dsr search)
        {
            var dsr = NaturalDbContext.Dsrs
                 .Include(c => c.ExecutiveNavigation)
                 .Include(c => c.DistributorNavigation)
                 .Include(c => c.RetailorNavigation)
                  .Include(c => c.OrderByNavigation)
                      .Where(c =>
                    (string.IsNullOrEmpty(search.Executive) || c.Executive ==search.Executive) &&
                    (string.IsNullOrEmpty(search.Distributor) || c.Distributor ==search.Distributor) &&
                    (string.IsNullOrEmpty(search.Retailor) || c.Retailor ==search.Retailor) &&
                    (string.IsNullOrEmpty(search.OrderBy) || c.OrderBy == search.OrderBy)
                    &&
                    
                      (search.CreatedDate == null || c.CreatedDate.Date >= search.CreatedDate.Date && c.CreatedDate.Date <= search.ModifiedDate.Date))
                      
                .Select(c => new Dsr
                {
                    Id =  c.Id,
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
                Include(D => D.Executive).Where(c => c.ExecutiveId == ExecutiveId).ToListAsync();
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
                Include(D => D.Distributor).Where(c => c.DistributorId == DistributorId).ToListAsync();
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
                     .Where(c => c.Id == dsrid)
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
            //DateTime threeDaysAgo = DateTime.Today.AddDays(-3);

            var dsr = await NaturalDbContext.Dsrs
                   .Include(c => c.ExecutiveNavigation)
                   .Include(c => c.DistributorNavigation)
                   .Include(c => c.RetailorNavigation)
                   .Include(c => c.OrderByNavigation)
                                      //.Where(c => c.DistributorNavigation.Id == distributorId  && c.CreatedDate >= threeDaysAgo && c.CreatedDate <= DateTime.Today)
                   .Where(c => c.DistributorNavigation.Id == distributorId)

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




        public async Task<IEnumerable<Dsr>> GetRetailorDetailsByDate(string distributorId, DateTime date)
        {
            var retailorList = await NaturalDbContext.Dsrs
                .Include(c => c.ExecutiveNavigation)
                .Include(c => c.DistributorNavigation)
                .Include(c => c.RetailorNavigation)
                .Include(c => c.OrderByNavigation)
                .Where(c => c.Distributor == distributorId)
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
                   .Where(c => c.ExecutiveNavigation.Id == executiveId)
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




        //public async Task<IEnumerable<Dsr>> SearchDsrByDistributorIds(string distributorId, DateTime startDate)
        //{
        //    var dsrs = await NaturalDbContext.Dsrs
        //        .Include(c => c.ExecutiveNavigation)
        //        .Include(c => c.DistributorNavigation)
        //        .Include(c => c.RetailorNavigation)
        //        .Include(c => c.OrderByNavigation)
        //        .Where(c => c.Distributor == distributorId && c.CreatedDate >= startDate)
        //        .Select(c => new Dsr
        //        {
        //            Id = c.Id,
        //            Executive = string.Concat(c.ExecutiveNavigation.FirstName, "", c.ExecutiveNavigation.LastName),
        //            Distributor = string.Concat(c.DistributorNavigation.FirstName, "", c.DistributorNavigation.LastName),
        //            Retailor = string.Concat(c.RetailorNavigation.FirstName, "", c.RetailorNavigation.LastName),
        //            OrderBy = string.Concat(c.OrderByNavigation.FirstName, "", c.OrderByNavigation.LastName),
        //            CreatedDate = c.CreatedDate,
        //            ModifiedDate = c.ModifiedDate,
        //            TotalAmount = c.TotalAmount
        //        }).ToListAsync();
        //    return dsrs;
        //}



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

        

        private NaturalsContext NaturalDbContext
        {
            get
            {
                return Context as NaturalsContext;
            }
        }
    }
}
