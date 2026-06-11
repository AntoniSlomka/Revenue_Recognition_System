namespace Revenue_Recognition_System.DTOs.Get
{
    public interface IGetCustomerSimpleDTO
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
