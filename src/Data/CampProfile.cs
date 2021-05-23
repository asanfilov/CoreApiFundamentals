using AutoMapper;

using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>()
                .ForMember( c => c.Venue, o => o.MapFrom( m => m.Location.VenueName ) )
                .ReverseMap();

            this.CreateMap<Talk, TalkModel>()
                .ReverseMap()
                //do not overwrite Talk properties with what's in TalkModel (that's why it needs to be after .ReverseMap)
                .ForMember( t => t.Camp, opt => opt.Ignore() )
                .ForMember( t => t.Speaker, opt => opt.Ignore() );

            this.CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
    }
}
