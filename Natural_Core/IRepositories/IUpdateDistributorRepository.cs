
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IUpdateDistributorRepository : IRepository<UpdateDistributor>
    {
        Task<Distributor> GetDistributorBylId(string Id);
    }
}
