using Revenue_Recognition_System.DTOs.Get;

namespace Revenue_Recognition_System.Repositories
{
    public interface ISoftwareRepository
    {
        Task<GetSoftwareDTO> GetSoftwareById(int id);
    }
}
