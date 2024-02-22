using AutoMapper;
using Microsoft.CodeAnalysis;
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
    public class DsrRepository : Repository<Dsr>, IDsrRepository
    {
        public DsrRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Dsr>> GetAllDsrAsync()
        {
            var dsr = from Dsr in NaturalDbContext.Dsrs
                      join executive in NaturalDbContext.Executives on Dsr.Executive equals executive.Id
                      join distributor in NaturalDbContext.Distributors on Dsr.Distributor equals distributor.Id
                      join retailer in NaturalDbContext.Retailors on Dsr.Retailor equals retailer.Id
                      join ordby in NaturalDbContext.Logins on Dsr.OrderByNavigation.UserName equals ordby.UserName
                      select new
                      {
                          dsrs = Dsr,
                          Executive = executive,
                          Distributor = distributor,
                          Retailor = retailer,
                          OrderByNavigation = ordby
                         
                      };

            var dsrs = await dsr.ToListAsync();
            var result = dsr.Select(c => new Dsr
            {
                Id = c.dsrs.Id,
                Executive = string.Concat(c.Executive.FirstName, c.Executive.LastName),
                Distributor = string.Concat(c.Distributor.FirstName, c.Distributor.LastName),
                Retailor = string.Concat(c.Retailor.FirstName, c.Retailor.LastName),
                OrderBy = string.Concat(c.OrderByNavigation.FirstName, c.OrderByNavigation.LastName),
                TotalAmount=c.dsrs.TotalAmount,
                CreatedDate = DateTime.Now,


            }).ToList();

            return result;


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
                    (string.IsNullOrEmpty(search.OrderBy) || c.Retailor == search.OrderBy)
                    &&
                    (search.CreatedDate == null || c.CreatedDate.Date == search.CreatedDate.Date))
       
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


        public async Task<Dsr> GetDetails(string dsrid)
        {
            var dsrQuery = from Dsr in NaturalDbContext.Dsrs
                           join executive in NaturalDbContext.Executives on Dsr.Executive equals executive.Id
                           join distributor in NaturalDbContext.Distributors on Dsr.Distributor equals distributor.Id
                           join retailer in NaturalDbContext.Retailors on Dsr.Retailor equals retailer.Id
                           join ordby in NaturalDbContext.Logins on Dsr.OrderByNavigation.UserName equals ordby.UserName
                           select new
                           {
                               dsrs = Dsr,
                               Executive = executive,
                               Distributor = distributor,
                               Retailor = retailer,
                               OrderByNavigation = ordby,
                           };

            var dsrDetails = await dsrQuery.ToListAsync();
            var result = dsrDetails.Select(c => new Dsr
            {
                Id = c.dsrs.Id,
                Executive = string.Concat(c.Executive.FirstName, c.Executive.LastName),
                Distributor = string.Concat(c.Distributor.FirstName, c.Distributor.LastName),
                Retailor = string.Concat(c.Retailor.FirstName, c.Retailor.LastName),
                OrderBy = string.Concat(c.OrderByNavigation.FirstName, c.OrderByNavigation.LastName),
                CreatedDate = DateTime.Now,


            }).ToList();

            var productDetails = await GetProductDetailsByDsrIdAsync(dsrid);
            var details = result.Where(c => c.Id == dsrid).First();
           

            return details;
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




        private NaturalsContext NaturalDbContext
        {
            get
            {
                return Context as NaturalsContext;
            }
        }
    }
}
