using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs
{
    public class GetCompanyCustomerSimpleDTO : IGetCustomerSimpleDTO
    {
        public int Id { get; set; }
        public string KrsNumber { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
