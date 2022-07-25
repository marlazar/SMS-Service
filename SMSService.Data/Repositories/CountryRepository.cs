using ServiceStack.Data;
using ServiceStack.OrmLite;
using SMSService.Core.Interfaces;
using SMSService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SMSService.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private IDbConnectionFactory _dbConnectionFactory { get; set; }

        public CountryRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public Task<Country> GetByCodeAsync(string code)
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                return db.SingleAsync<Country>(x => x.CountryCode == code);
            }
        }

        public Task<Country> GetByMobileCodeAsync(string mobileCode)
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                return db.SingleAsync<Country>(x => x.MobileCountryCode == mobileCode);
            }
        }

        public Task<List<Country>> GetAllAsync()
        {
            using (var db = _dbConnectionFactory.OpenDbConnection())
            {
                return db.SelectAsync<Country>();
            }
        }
    }
}
