using AutoMapper;
using Catalog.API.Entities;
using EventBus.Messages.Events;

namespace Ordering.API.Mapper
{
	public class CheckProducts : Profile
	{
		public CheckProducts()
		{
			CreateMap<CheckOut, BasketCheckoutEvent>().ReverseMap();
			CreateMap<Order, OrderEvent>().ReverseMap();
			CreateMap<EventBus.Messages.Events.ShoppingCartItem, Catalog.API.Entities.ShoppingCartItem>().ReverseMap();
		}
	}
}
