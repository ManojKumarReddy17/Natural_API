using Natural_Core.Models;
using Natural_Core.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Natural_Core;
using Natural_Data.Models;
using Microsoft.Extensions.Logging;
using System;


#nullable disable

namespace Natural_Data.Repositories
{
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        private readonly ILogger<LoginRepository> _logger;
        public LoginRepository(NaturalsContext context, ILogger<LoginRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public LoginRepository(DbContext context) : base(context)
        {
        }

        public async Task<List<Login>> GetDetailsAsync()
        {
            try
            {
                return await NaturalDbContext.Logins.ToListAsync();
            }
            
              catch (Exception ex)
            {
                _logger.LogError(" LoginRepository - GetDetailsAsync", ex.Message);
                return null;

            }
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
