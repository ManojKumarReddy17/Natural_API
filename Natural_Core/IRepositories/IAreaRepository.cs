using Natural_Core.Models;
using Natural_Core.S3Models;
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
        Task<Area> GetAreasId(string AreaId);


    }
}
