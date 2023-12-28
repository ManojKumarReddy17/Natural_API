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
    public class DSRRepository : Repository<Dsr>, IDSRRepository
    {
        private readonly IMapper _mapper;
        public DSRRepository(NaturalsContext context) : base(context)
        {
            
        }
        public async Task<IEnumerable<Dsr>> GetAllAsync(string dsrId)
        {
            var dsr = await NaturalDbContext.Dsrs
                .Where(d => d.Id == dsrId)
                .Include(d => d.DistributorNavigation)
                .Include(d => d.ExecutiveNavigation)
                .Include(d => d.OrderByNavigation)
                .Include(d => d.RetailorNavigation)
                .Include(d => d.Dsrdetails)
                .ThenInclude(d => d.ProductNavigation)
                .ToListAsync();
            var result = dsr.Select(c => new Dsr
            {
                Id = c.Id,
                Executive = c.Executive,
                Distributor = c.Distributor,
                Retailor = c.Retailor,
                OrderBy = c.OrderBy,
                CreatedDate = c.CreatedDate,
                TotalAmount = c.TotalAmount,
            }).ToList();
            return dsr;
        }






        public async Task<IEnumerable<Product>> GetProductDetailsByDsrIdAsync(string dsrId)
        {
            var productDetails = await NaturalDbContext.Dsrdetails
                .Where(d => d.Dsr == dsrId)
                .Select(d => new Product
                {
                    Id = d.ProductNavigation.Id,                
                    product = d.ProductNavigation.product,      
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
            var dsrlist= await NaturalDbContext.Dsrs.Include(D=> D.DistributorNavigation).
                                               Include(D=> D.RetailorNavigation).
                                               Include(D=> D.ExecutiveNavigation).
                FirstOrDefaultAsync(c=> c.Id== DsrId);
                                               
            if(dsrlist != null)
            {
                var result = new Dsr
                {
                    Id = dsrlist.Id,
                    Executive = string.Concat(dsrlist.ExecutiveNavigation.FirstName, " ", dsrlist.ExecutiveNavigation.LastName),
                    Distributor = string.Concat(dsrlist.DistributorNavigation.FirstName, " ", dsrlist.DistributorNavigation.LastName),
                    Retailor = string.Concat(dsrlist.RetailorNavigation.FirstName, " ", dsrlist.RetailorNavigation.LastName),
                    CreatedDate = dsrlist.CreatedDate

                };
                return result;
    
            }
            else
            {
                Id = c.dsrs.Id,
                Executive = string.Concat(c.Executive.FirstName, c.Executive.LastName),
                Distributor = string.Concat(c.Distributor.FirstName, c.Distributor.LastName),
                Retailor=string.Concat(c.Retailor.FirstName,c.Retailor.LastName),
                OrderBy=string.Concat(c.OrderByNavigation.FirstName,c.OrderByNavigation.LastName),
                CreatedDate= DateTime.Now,
                TotalAmount=c.dsrs.TotalAmount
               
             
            }).ToList();



            var productDetails = await GetProductDetailsByDsrIdAsync(dsrid);
            var details = result.Where(c => c.Id == dsrid).First();
            details.ProductDetails = (IEnumerable<Product>)productDetails;

            return details;
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
