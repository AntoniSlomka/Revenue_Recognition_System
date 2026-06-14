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

            var total = signedContracts.Sum(c => c.Payments.Sum(p => p.Value));

            return total;
        }

        private async Task<decimal> CalculateTotalRevenue()
        {
            var contracts = await _contractRepository.GetAllContracts();

            var signedContracts = contracts.Where(c => c.Status == Enums.ContractStatus.Signed).ToList();

            var total = signedContracts.Sum(c => c.Payments.Sum(p => p.Value));

            return total;
        }

        private async Task<decimal> CalculatePredictedProductRevenue(int id)
        {
            var contracts = await _contractRepository.GetAllContractsBySoftwareId(id);

            var signedContracts = contracts
                .Where(c => (c.Status == Enums.ContractStatus.Signed || c.Status == Enums.ContractStatus.Created))
                .ToList();

            var total = signedContracts.Sum(c => c.Payments.Sum(p => p.Value));

            return total;
        }

        private async Task<decimal> CalculatePredictedTotalRevenue()
        {
            var contracts = await _contractRepository.GetAllContracts();

            var signedContracts = contracts
                .Where(c => (c.Status == Enums.ContractStatus.Signed || c.Status == Enums.ContractStatus.Created))
                .ToList();

            var total = signedContracts.Sum(c => c.Payments.Sum(p => p.Value));

            return total;
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

        private async Task<decimal> GetExchangeRateByCode(string code)
        {
            var client = _httpClientFactory.CreateClient("CurrencyExchange");
            var exchange = await client.GetFromJsonAsync<GetCurrencyExchangeDTO>($"api/exchangerates/rates/a/{code}/?format=json");

            if (exchange == null) throw new ExternalServerExcpetion($"Exchange service error.");

            return exchange.Rates.First().Mid;
        }

    }
}
