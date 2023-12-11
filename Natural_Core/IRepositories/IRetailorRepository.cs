using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IRetailorRepository : IRepository<Retailor>
    {

        Task<IEnumerable<Retailor>> GetAllRetailorsAsync();
       
        Task<Retailor> GetWithRetailorsByIdAsync(string id);
        Task UpdateRetailorAsync(Retailor retailor, Retailor existingRetailor);

    }
}