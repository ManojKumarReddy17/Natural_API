using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IRetailorRepository : IRepository<Retailor>
    {

        Task<IEnumerable<Retailor>> GetAllRetailorsAsync(SearchModel? search, bool? NonAssign);
        Task<GetRetailor> GetRetailorDetailsByIdAsync(string id);
        Task UpdateRetailorAsync(Retailor retailor, Retailor existingRetailor);
    }
}