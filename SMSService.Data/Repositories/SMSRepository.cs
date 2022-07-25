using ServiceStack.Data;
using ServiceStack.OrmLite;
using SMSService.Core.Interfaces;
using SMSService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSService.Data.Repositories
{
    public class SMSRepository : ISMSRepository
    {
        private IDbConnectionFactory _dbConnectionFactory { get; set; }

        public SMSRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task AddAsync(SMS sms)
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                sms.SendDate = DateTime.UtcNow;
                await db.InsertAsync(sms);
            }
        }

        public Task<List<SMS>> FilterByDateAsync(DateTime fromDate, DateTime toDate, int skip, int take)
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                var filterQuery = db.From<SMS>()
                    .Where(x => x.SendDate >= fromDate && x.SendDate <= toDate)
                    .Skip(skip)
                    .Take(take);

                return db.SelectAsync(filterQuery);
            }
        }

        public async Task<IEnumerable<SMSGroupBy>> GroupByCountryDayAsync(DateTime fromDate, DateTime toDate, string countries)
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                var groupByQuery = db.SelectAsync<SMSGroupBy>(@"
                            SELECT MobileCountryCode, DATE(SendDate), Count(*)
                            FROM mittosample.sms
                            GROUP BY MobileCountryCode, DATE(SendDate)");

                IEnumerable<SMSGroupBy> result = await groupByQuery;

                if (!string.IsNullOrEmpty(countries))
                {
                    var countryList = countries.Split(',');
                    result = (await groupByQuery).Where(x => countryList.Contains(x.MobileCountryCode));
                }

                result = result.Where(x => x.SendDate.Date >= fromDate.Date && x.SendDate.Date <= toDate.Date);

                return result;
            }
        }
    }
}
