﻿namespace HotelListing.API.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;

        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}