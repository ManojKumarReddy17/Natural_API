using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Core.IServices
{
    public interface IStateService
    {
        Task<IEnumerable<State>> GetStatesAsync();
    }
}
