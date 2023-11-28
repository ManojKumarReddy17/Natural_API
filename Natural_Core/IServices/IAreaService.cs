using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Core.IServices
{
    public interface IAreaService
    {
        Task<IEnumerable<Area>> GetAreasAsync();

        Task<IEnumerable<Area>> GetAreasWithCityID(string CityId);
    }
}


