using Microsoft.EntityFrameworkCore;
using Natural_API.Models;
using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Data.Repositories
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<State>> GetAllStatesAsync()
        {
            return await NaturalDbContext.States.ToListAsync();
        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
