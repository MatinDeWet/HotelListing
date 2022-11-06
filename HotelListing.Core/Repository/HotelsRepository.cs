using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.EF;

namespace HotelListing.API.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        private readonly PostGreContext _context;

        public HotelsRepository(PostGreContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }
    }
}
