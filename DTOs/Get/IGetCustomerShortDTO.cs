namespace Revenue_Recognition_System.DTOs.Get
{
    public interface IGetCustomerShortDTO
    {
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
