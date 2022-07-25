using SMSService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SMSService.Core.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country> GetByCodeAsync(string code);

        Task<Country> GetByMobileCodeAsync(string mobileCode);

        Task<List<Country>> GetAllAsync();
    }
}
