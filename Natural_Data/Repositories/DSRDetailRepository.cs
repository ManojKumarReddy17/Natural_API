using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


#nullable disable
namespace Natural_Data.Repositories
{
    public class DSRDetailRepository: Repository<Dsrdetail>, IdsrDetailsRepository
    {
        public DSRDetailRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Dsrdetail>> GetAllDsrdetailsAsync()
        {
            return await NaturalDbContext.Dsrdetails.ToListAsync();
        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
