using Natural_Core.Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Core.IServices
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<IEnumerable<City>> GetCitywithStateId(string StateId);
        Task <City>GetCityWithId(string CityId);
        Task<ProductResponse> InsertWithCity(City city);

        Task<ProductResponse> UpdateWithCity(City UpdateWithCity);
        Task<ResultResponse> DeleteCity(string CityId);
    }
}
