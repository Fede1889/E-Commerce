using System.Collections.Generic;

namespace Basket.API.Entities
{
    public class BasketCheckout
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        //Dati per l'invio
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        //Articoli nel carrello
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }
}
