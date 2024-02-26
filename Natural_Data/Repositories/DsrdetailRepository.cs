using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;

namespace Natural_Data.Repositories
{
	public class DsrdetailRepository: Repository<Dsrdetail>, IDsrdetailRepository
    {
		public DsrdetailRepository(NaturalsContext context) : base(context)
        {
		}


        public async Task<IEnumerable<Dsrdetail>> GetDsrDetailsByDsrIdAsync(string dsrId)
        {
            var productDetails = await NaturalDbContext.Dsrdetails
                 .Include(c => c.ProductNavigation)
                .Where(d => d.Dsr == dsrId)
                .Select(d => new Dsrdetail
                {
                    Dsr = d.Dsr,
                    Product = d.ProductNavigation.ProductName,
                    Quantity = d.Quantity,
                    Price = d.Price,
                    Id = d.Id

                })
                .ToListAsync();

            return productDetails;
        }

        //public async Task<IEnumerable<GetProduct>> GetDsrDetailsByDsrIdAsync(string dsrId)
        //{
        //    var productDetails = await NaturalDbContext.Dsrdetails
        //         .Include(c => c.ProductNavigation)
        //        .Where(d => d.Dsr == dsrId)
        //        .Select(d => new GetProduct
        //        {
        //            Id = d.Dsr,
        //            ProductName = d.ProductNavigation.ProductName,
        //            Quantity = d.Quantity,
        //            Price = d.Price,
        //            Category =d.ProductNavigation.CategoryNavigation.CategoryName

        //        })
        //        .ToListAsync();

        //    return productDetails;
        //}




        private NaturalsContext NaturalDbContext
        {
            get
            {
                return Context as NaturalsContext;
            }
        }

    }
}

