using ServiceStack;
using SMSService.Core.Interfaces;
using SMSService.ServiceModels.DTO_Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SMSService.Services.Services
{
    public class CountryServices : Service
    {
        private ICountryRepository _countryRepository { get; set; }

        public CountryServices(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<GetCountriesResponse> Get(GetCountries request)
        {
            var countryList = await _countryRepository.GetAllAsync();

            var countryResponses = new List<CountryResponse>();

            foreach (var item in countryList)
            {
                countryResponses.Add(new CountryResponse
                {
                    Mcc = item.MobileCountryCode,
                    Cc = item.CountryCode,
                    Name = item.Name,
                    PricePerSMS = decimal.Parse(item.PricePerSMS.ToString().TrimEnd('0').TrimEnd('.'))
                });
            }

            return new GetCountriesResponse { Result = countryResponses };
        }
    }
}
