using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

#nullable disable

namespace Natural_Services
{
    public class StateService : IStateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<State>> GetStatesAsync()
        {

            var result = await _unitOfWork.StateRepo.GetAllAsync();
            var presentSates = result.Where(c => c.IsDeleted == false).ToList();
            return presentSates;

        }
    }
}
