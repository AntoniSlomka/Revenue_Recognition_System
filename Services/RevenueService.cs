using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.Exceptions;
using Revenue_Recognition_System.Repositories;

namespace Revenue_Recognition_System.Services
{
    public class RevenueService : IRevenueService
    {
        private readonly IContractRepository _contractRepository;
        private readonly ISoftwareRepository _softwareRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public RevenueService(IContractRepository contractRepository, ISoftwareRepository softwareRepository, IHttpClientFactory httpClientFactory)
        {
            _contractRepository = contractRepository;
            _softwareRepository = softwareRepository;
            _httpClientFactory = httpClientFactory;
        }

        private async Task<decimal> CalculateProductRevenue(int id)
        {
            var contracts = await _contractRepository.GetAllContractsBySoftwareId(id);

            var signedContracts = contracts.Where(c => c.Status == Enums.ContractStatus.Signed).ToList();

            var total = signedContracts.Sum(c => c.FinalPrice);

            return total;
        }

        private async Task<decimal> CalculateTotalRevenue()
        {
            var contracts = await _contractRepository.GetAllContracts();

            var signedContracts = contracts.Where(c => c.Status == Enums.ContractStatus.Signed).ToList();

            var total = signedContracts.Sum(c => c.FinalPrice);

            return total;
        }

        private async Task<decimal> CalculatePredictedProductRevenue(int id)
        {
            var contracts = await _contractRepository.GetAllContractsBySoftwareId(id);

            var activeContracts = contracts
                .Where(c => (c.Status.ToString() == "Signed" || c.Status.ToString() == "Created"))
                .ToList();

            var total = activeContracts.Sum(c => c.FinalPrice);

            return total;
        }

        private async Task<decimal> CalculatePredictedTotalRevenue()
        {
            var contracts = await _contractRepository.GetAllContracts();

            var activeContracts = contracts
                .Where(c => (c.Status.ToString() == "Signed" || c.Status.ToString() == "Created"))
                .ToList();

            var total = activeContracts.Sum(c => c.FinalPrice);

            return total;
        }

        private async Task<decimal> GetExchangeRateByCode(string code)
        {
            var client = _httpClientFactory.CreateClient("CurrencyExchange");
            var exchange = await client.GetFromJsonAsync<GetCurrencyExchangeDTO>($"api/exchangerates/rates/a/{code}/?format=json");

            if (exchange == null) throw new ExternalServerExcpetion($"Exchange service error.");

            return exchange.Rates.First().Mid;
        }

        public async Task<GetProductRevenueDTO> GetProductRevenueById(int id, string? code)
        {
            var product = await _softwareRepository.GetSoftwareById(id);
            var total = await CalculateProductRevenue(id);

            if (code != null)
            {
                var exchangeRate = await GetExchangeRateByCode(code);

                return new GetProductRevenueDTO
                {
                    SoftwareId = product.SoftwareId,
                    Name = product.Name,
                    RevenueInPLN = total,
                    AlternateCurrency = code,
                    RevenueInAlternateCurrency = total / exchangeRate
                };
            }
            return new GetProductRevenueDTO
            {
                SoftwareId = product.SoftwareId,
                Name = product.Name,
                RevenueInPLN = total
            };
        }

        public async Task<GetTotalRevenue> GetTotalRevenue(string? code)
        {
            var total = await CalculateTotalRevenue();

            if (code != null)
            {
                var exchangeRate = await GetExchangeRateByCode(code);

                return new GetTotalRevenue
                {
                    RevenueInPLN = total,
                    AlternateCurrency = code,
                    RevenueInAlternateCurrency = ((int)(total / exchangeRate * 100)) / 100.0M
                };
            }
            return new GetTotalRevenue
            {
                RevenueInPLN = total
            };
        }

        public async Task<GetProductRevenueDTO> GetPredictedProductRevenueById(int id, string? code)
        {
            var product = await _softwareRepository.GetSoftwareById(id);
            var total = await CalculatePredictedProductRevenue(id);

            if (code != null)
            {
                var exchangeRate = await GetExchangeRateByCode(code);

                return new GetProductRevenueDTO
                {
                    SoftwareId = product.SoftwareId,
                    Name = product.Name,
                    RevenueInPLN = total,
                    AlternateCurrency = code,
                    RevenueInAlternateCurrency = total / exchangeRate
                };
            }
            return new GetProductRevenueDTO
            {
                SoftwareId = product.SoftwareId,
                Name = product.Name,
                RevenueInPLN = total
            };
        }

        public async Task<GetTotalRevenue> GetPredictedTotalRevenue(string? code)
        {
            var total = await CalculatePredictedTotalRevenue();

            if (code != null)
            {
                var exchangeRate = await GetExchangeRateByCode(code);

                return new GetTotalRevenue
                {
                    RevenueInPLN = total,
                    AlternateCurrency = code,
                    RevenueInAlternateCurrency = ((int)(total / exchangeRate * 100)) / 100.0M
                };
            }
            return new GetTotalRevenue
            {
                RevenueInPLN = total
            };
        }

    }
}
