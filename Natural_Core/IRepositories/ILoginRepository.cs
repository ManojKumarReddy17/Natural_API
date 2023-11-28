using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Core.IRepositories
{
    public interface ILoginRepository : IRepository<Login>
    {
        Task<List<Login>> GetDetailsAsync();
    }
}
