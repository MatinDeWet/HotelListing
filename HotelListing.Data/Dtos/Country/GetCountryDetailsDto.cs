namespace HotelListing.API.Data
{
    public class GetCountryDetailsDto : BaseCountryDto
    {
        public int Id { get; set; }

        public List<GetHotelDto> Hotels { get; set; }
    }
}
