using ServiceStack;
using SMSService.Core.Interfaces;
using SMSService.Core.Models;
using SMSService.ServiceModels.DTO_Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SMSService.Services.Services
{
    public class SMSServices : Service
    {
        private ISMSRepository _smsRepository { get; set; }
        private ICountryRepository _countryRepository { get; set; }

        public SMSServices(ISMSRepository smsRepository, ICountryRepository countryRepository)
        {
            _smsRepository = smsRepository;
            _countryRepository = countryRepository;
        }

        public async Task<SendSMSResponse> Get(SendSMS request)
        {
            var sms = new SMS
            {
                Receiver = request.To,
                Sender = request.From,
                Text = request.Text
            };

            await ExtractMobileCountryCode(sms);

            await _smsRepository.AddAsync(sms);

            return new SendSMSResponse { State = StateEnum.Success };
        }

        private async Task ExtractMobileCountryCode(SMS sms)
        {
            var countryCode = sms.Receiver?.TrimStart('+', '0')?.Substring(0, 2);

            if (!string.IsNullOrEmpty(countryCode))
            {
                var country = await _countryRepository.GetByCodeAsync(countryCode);
                sms.MobileCountryCode = country.MobileCountryCode;
            }
        }

        public async Task<GetSentSMSResponse> Get(GetSentSMS request)
        {
            var filteredList = await _smsRepository.FilterByDateAsync(
                DateTime.Parse(request.DateTimeFrom),
                DateTime.Parse(request.DateTimeTo),
                request.Skip,
                request.Take);

            var sentSMSResponses = new List<SentSMSResponse>();

            foreach (var item in filteredList)
            {
                var itemCountry = (await _countryRepository.GetByMobileCodeAsync(item.MobileCountryCode));

                decimal itemPrice = itemCountry != null ? itemCountry.PricePerSMS : 0;

                sentSMSResponses.Add(new SentSMSResponse
                {
                    DateTime = item.SendDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Mcc = item.MobileCountryCode,
                    From = item.Sender,
                    To = item.Receiver,
                    Price = decimal.Parse(itemPrice.ToString().TrimEnd('0').TrimEnd('.'))
                });
            }

            return new GetSentSMSResponse
            {
                TotalCount = sentSMSResponses.Count,
                Items = sentSMSResponses,
                State = StateEnum.Success
            };
        }

        public async Task<GetStatisticsResponse> Get(GetStatistics request)
        {
            var filteredSMSList = await _smsRepository.GroupByCountryDayAsync(
                DateTime.Parse(request.DateFrom),
                DateTime.Parse(request.DateTo),
                request.MccList);

            var statisticsResponses = new List<StatisticsResponse>();

            foreach (var item in filteredSMSList)
            {
                var itemCountry = await _countryRepository.GetByMobileCodeAsync(item.MobileCountryCode);

                statisticsResponses.Add(new StatisticsResponse
                {
                    Count = item.Count,
                    Day = item.SendDate.Date.ToString("yyyy-MM-dd"),
                    Mcc = item.MobileCountryCode,
                    PricePerSMS = decimal.Parse(itemCountry.PricePerSMS.ToString().TrimEnd('0').TrimEnd('.')),
                    TotalPrice = decimal.Parse((item.Count * itemCountry.PricePerSMS).ToString().TrimEnd('0').TrimEnd('.'))
                });
            }

            return new GetStatisticsResponse { Items = statisticsResponses };
        }
    }
}
