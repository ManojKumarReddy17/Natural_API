using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
<<<<<<< HEAD
    public  interface IExecutiveRepository :IRepository<Executive>
    {
        Task<IEnumerable<Executive>> GetAllExecutiveAsync();
=======
    public interface IExecutiveRepository:IRepository<Executive>
    {
        Task<List<Executive>> GetAllExectivesAsync();
        Task<Executive> GetWithExectiveByIdAsync(string id);
>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa
    }
}
