using SMSService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SMSService.Core.Interfaces
{
    public interface ISMSRepository
    {
        Task AddAsync(SMS sms);

        Task<List<SMS>> FilterByDateAsync(DateTime fromDate, DateTime toDate, int skip, int take);

        Task<IEnumerable<SMSGroupBy>> GroupByCountryDayAsync(DateTime fromDate, DateTime toDate, string countries);
    }
}
