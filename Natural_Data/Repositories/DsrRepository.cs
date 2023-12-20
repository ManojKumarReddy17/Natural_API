using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Data.Repositories
{
    public class DsrRepository: Repository<Dsr>,IDSRRepository
    {
        public DsrRepository(NaturalsContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Dsr>> GetAllDsrAsync()
        {
            return await NaturalDbContext.Dsrs.ToListAsync();
        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }

}
