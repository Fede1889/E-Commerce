using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.API.Mapper
{
    public class BasketProfile : Profile
	{
		public BasketProfile()
		{
			CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
			CreateMap<Basket.API.Entities.ShoppingCartItem, EventBus.Messages.Events.ShoppingCartItem>().ReverseMap();
		}
	}
}
