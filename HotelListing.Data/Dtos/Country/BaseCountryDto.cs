using Microsoft.Build.Framework;

namespace HotelListing.API.Data
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}
