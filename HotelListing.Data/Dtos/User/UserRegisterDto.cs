using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Data.Dtos
{
    public class UserRegisterDto : UserLoginDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
