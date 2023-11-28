using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable


namespace Natural_Core.IRepositories
{
    public interface IAreaRepository : IRepository<Area>
    {

        Task<IEnumerable<Area>> GetAllAreasAsync();
    }
}
