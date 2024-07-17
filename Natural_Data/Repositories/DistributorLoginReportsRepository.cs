using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Data.Repositories
{
    public class DistributorLoginReportsRepository : Repository<DistributorReport>, IDistributorLoginReportsRepository
    {
        public DistributorLoginReportsRepository(NaturalsContext context) : base(context)
        {

        }

        public async Task<IEnumerable<DistributorReport>> GetById(DistributorLoginReports DSReport)
        {
            //var saleReport = await NaturalsDbContext.DistributorReports
            //    .FromSqlInterpolated($"CALL Naturals.DistributorLogin({DSReport.Distributor}, {DSReport.Retailor}, {DSReport.StartDate.Date}, {DSReport.EndDate.Date})")
            //    .ToListAsync();

            //return saleReport;
            var saleReport = await NaturalsDbContext.DistributorReports
       .FromSqlInterpolated($@"
            CALL Naturals.DistributorLogin(
                {DSReport.Distributor}, 
                {DSReport.Retailor ?? ""}, 
                {DSReport.StartDate}, 
                {DSReport.EndDate}
            )")
       .ToListAsync();

            return saleReport;
        }


        private NaturalsContext NaturalsDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
