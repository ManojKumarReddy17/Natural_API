using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

#nullable disable
namespace Natural_Data.Repositories
{
    public class DsrRepository : Repository<Dsr>, IDSRRepository
    {
        public DsrRepository(NaturalsContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Dsr>> GetAllDsrAsync()
        {
            var dsrs = await (
                from dsr in NaturalDbContext.Dsrs
                join executive in NaturalDbContext.Executives on dsr.Executive equals executive.Id
                join distributor in NaturalDbContext.Distributors on dsr.Distributor equals distributor.Id
                join retailor in NaturalDbContext.Retailors on dsr.Retailor equals retailor.Id
                join orderyby in NaturalDbContext.Logins on dsr.OrderBy equals orderyby.Id
                select new
                {
                    dsr = dsr,
                    Executive = executive,
                    Distributor = distributor,
                    Retailor = retailor,
                    orderyby = orderyby
                }
            ).ToListAsync();

            var result = dsrs.Select(c => new Dsr
            {
                Id = c.dsr.Id,
                TotalAmount = c.dsr.TotalAmount,          
                OrderBy= string.Concat(c.orderyby.FirstName,"",c.orderyby.LastName),
                Executive= string.Concat(c.Executive.FirstName ," ",c.Executive.LastName),
                Distributor=string.Concat(c.Distributor.FirstName ,"",c.Distributor.LastName),
                Retailor=string.Concat(c.Retailor.FirstName,"",c.Retailor.LastName),
                CreatedDate = c.dsr.CreatedDate,
                ModifiedDate =c.dsr.ModifiedDate
                       });
        
            return result;
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
