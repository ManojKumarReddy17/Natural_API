using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
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
        public DSRRepository(NaturalsContext context) : base(context)
        {
        }

        public async Task<Dsr> GetDsrDetailsByIdAsync(string DsrId)
        {
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
                return null;
            }
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
