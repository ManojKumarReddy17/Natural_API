using Natural_Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable


namespace Natural_Core
{
    public interface IUnitOfWork : IDisposable
    {
        ILoginRepository Login { get; }
        IDistributorRepository DistributorRepo { get; }
        ICityRepository CityRepo { get; }
        IStateRepository StateRepo { get; }
        IAreaRepository AreaRepo { get; }
        ICategoryRepository CategoryRepo { get; }
        IRetailorRepository  RetailorRepo { get; }
        IExecutiveRepository ExecutiveRepo { get; }
        Task<int> CommitAsync();


    }
}
