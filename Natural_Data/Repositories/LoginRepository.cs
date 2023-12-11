using Natural_Core.Models;
using Natural_Core.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Natural_Core;


#nullable disable

namespace Natural_Data.Repositories
{
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        public LoginRepository(NaturalsContext context) : base(context)
        {

        }

        public async Task<List<Login>> GetDetailsAsync()
        {
            return await NaturalDbContext.Logins.ToListAsync();
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
