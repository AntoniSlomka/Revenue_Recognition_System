using Revenue_Recognition_System.Entities;
using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs.Get
{
    public class GetSoftwareVersionDTO
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
    }
}
