using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NaturalsContext _context;

        private ILoginRepository _loginRepository;

        private IDistributorRepository _distributorRepository;
        private ICityRepository _cityRepository;
        private IAreaRepository _areaRepository;
        private IStateRepository _stateRepository;
        private ICategoryRepository _categoryRepository;
        private IRetailorRepository _retailorRepository;
       // private IUpdateDistributorRepository _updateRepository;


        public UnitOfWork(NaturalsContext context)
        {
            _context = context;
        }

        public ILoginRepository Login => _loginRepository = _loginRepository ?? new LoginRepository(_context);
        public IDistributorRepository DistributorRepo => _distributorRepository = _distributorRepository ?? new DistributorRepository(_context);
    
        public ICityRepository CityRepo => _cityRepository = _cityRepository ?? new CityRepository(_context);
        public IStateRepository StateRepo => _stateRepository = _stateRepository ?? new StateRepository(_context);

        public IAreaRepository AreaRepo => _areaRepository = _areaRepository ?? new AreaRepository(_context);

        public ICategoryRepository CategoryRepo => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        public IRetailorRepository RetailorRepo  => _retailorRepository = _retailorRepository ?? new RetailorRepository(_context);

        public IUpdateDistributorRepository UpdateDistributorRepository => throw new NotImplementedException();

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


