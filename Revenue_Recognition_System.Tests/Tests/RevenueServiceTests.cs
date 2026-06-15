using Moq;
using Revenue_Recognition_System.DTOs.Get;
using Revenue_Recognition_System.Repositories;
using Revenue_Recognition_System.Services;
using Xunit;

namespace Revenue_Recognition_System.Tests
{
    public class RevenueServiceTests
    {
        private readonly Mock<IContractRepository> _contractRepositoryMock;
        private readonly Mock<ISoftwareRepository> _softwareRepositoryMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly RevenueService _revenueServiceMock;

        public RevenueServiceTests()
        {
            _contractRepositoryMock = new Mock<IContractRepository>();
            _softwareRepositoryMock = new Mock<ISoftwareRepository>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            _revenueServiceMock = new RevenueService(
                _contractRepositoryMock.Object,
                _softwareRepositoryMock.Object,
                _httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task GetTotalRevenue()
        {
            var contracts = new List<GetContractShortDTO>
            {
                new() { Status = Enums.ContractStatus.Signed, FinalPrice = 4500 },
                new() { Status = Enums.ContractStatus.Created, FinalPrice = 3780 },
                new() { Status = Enums.ContractStatus.Signed, FinalPrice = 1290 },
                new() { Status = Enums.ContractStatus.Created, FinalPrice = 5600 },
                new() { Status = Enums.ContractStatus.Inactive, FinalPrice = 2000 },
            };

            _contractRepositoryMock.Setup(r => r.GetAllContracts())
                .ReturnsAsync(contracts);

            var result = await _revenueServiceMock.GetTotalRevenue(null);

            Assert.Equal(5790, result.RevenueInPLN);
        }

        [Fact]
        public async Task GetTotalPredictedRevenue()
        {
            var contracts = new List<GetContractShortDTO>
            {
                new() { Status = Enums.ContractStatus.Signed, FinalPrice = 4500 },
                new() { Status = Enums.ContractStatus.Created, FinalPrice = 3780 },
                new() { Status = Enums.ContractStatus.Signed, FinalPrice = 1290 },
                new() { Status = Enums.ContractStatus.Created, FinalPrice = 5600 },
                new() { Status = Enums.ContractStatus.Inactive, FinalPrice = 2000 },
            };

            _contractRepositoryMock.Setup(r => r.GetAllContracts())
                .ReturnsAsync(contracts);

            var result = await _revenueServiceMock.GetPredictedTotalRevenue(null);

            Assert.Equal(15170, result.RevenueInPLN);
        }
    }
}
