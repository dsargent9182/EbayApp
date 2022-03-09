using AutoMapper;
using DS.Domain.Models;


namespace DS.EbayAPI.AutoMapper.Profiles
{
	public class GiftCardProfile : Profile
	{
		public GiftCardProfile()
		{
			CreateMap<GiftCard, Dto.GiftCardDto>();
		}
	}
}
