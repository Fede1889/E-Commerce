using System.Collections.Generic;

namespace EventBus.Messages.Events
{
    //QUESTA E' LA STRUTTURA DELL'ORDINE
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        //Dati per l'invio
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    }
}
