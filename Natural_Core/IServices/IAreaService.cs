using Natural_Core.Models;
using Natural_Core.S3Models;
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
        Task<Area> GetAreasWithId(string AreaId);
         Task<ProductResponse> Insert(Area area);
        Task<ProductResponse> updateArea(Area updateArea);
        Task<ProductResponse> DeleteArea(String areaId);
    }
}


