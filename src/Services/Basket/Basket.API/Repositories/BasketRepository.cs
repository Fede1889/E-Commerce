using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDistributedCache _redisCache; //collegamento al DB

        public BasketRepository(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);    //ritorna il JSON con i prodotti nel carrello

            if (String.IsNullOrEmpty(basket))
                return null;            

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)

            /**
             * carica nel DB il JSON con i nuovi prodotti
             * [key = username, value = basket]
             */
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));     
            
            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);    //elimina il carrello
        }
    }
}
