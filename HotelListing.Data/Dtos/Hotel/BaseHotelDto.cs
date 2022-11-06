using Microsoft.Build.Framework;

namespace HotelListing.API.Data
{
    public abstract class BaseHotelDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public double Rating { get; set; }
    }
}
