namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetCurrencyExchangeDTO
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public GetRateDTO[] Rates { get; set; }
    }

    public class GetRateDTO
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public decimal Mid { get; set; }
    }

//    {
//    "table": "A",
//    "currency": "frank szwajcarski",
//    "code": "CHF",
//    "rates": [
//        {
//            "no": "112/A/NBP/2026",
//            "effectiveDate": "2026-06-12",
//            "mid": 4.6121
//        }
//    ]
//    }
}
