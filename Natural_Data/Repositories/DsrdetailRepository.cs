using System;
using Natural_Core.IRepositories;
using Natural_Core.Models;

namespace Natural_Data.Repositories
{
	public class DsrdetailRepository: Repository<Dsrdetail>, IDsrdetailRepository
    {
		public DsrdetailRepository(NaturalsContext context) : base(context)
        {
		}






        private NaturalsContext NaturalDbContext
        {
            get
            {
                return Context as NaturalsContext;
            }
        }

    }
}

