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
        IExecutiveRepository ExecutiveRepo { get; }
        ICityRepository CityRepo { get; }
        IStateRepository StateRepo { get; }
        IAreaRepository AreaRepo { get; }
        ICategoryRepository CategoryRepo { get; }
        IRetailorRepository  RetailorRepo { get; }
        Task<int> CommitAsync();


    }
}
