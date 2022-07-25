using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMSService.ServiceModels.DTO_Models
{
    [Route("/sms/send")]
    public class SendSMS : IReturn<SendSMSResponse>
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
    }

    public class SendSMSResponse
    {
        public StateEnum State { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/sms/sent")]
    public class GetSentSMS : IReturn<GetSentSMSResponse>
    {
        public string DateTimeFrom { get; set; }
        public string DateTimeTo { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetSentSMSResponse
    {
        public int TotalCount { get; set; }
        public List<SentSMSResponse> Items { get; set; }
        public StateEnum State { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class SentSMSResponse
    {
        public string DateTime { get; set; }
        public string Mcc { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Price { get; set; }
    }

    [Route("/statistics")]
    public class GetStatistics : IReturn<GetStatisticsResponse>
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string MccList { get; set; }
    }

    public class GetStatisticsResponse
    {
        public List<StatisticsResponse> Items { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class StatisticsResponse
    {
        public string Day { get; set; }
        public string Mcc { get; set; }
        public decimal PricePerSMS { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public enum StateEnum
    {
        Failed,
        Success
    }
}
