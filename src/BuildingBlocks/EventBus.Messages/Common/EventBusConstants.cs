namespace EventBus.Messages.Common
{
    public static class EventBusConstants
	{
		public const string BasketToCatalog = "basketcheckout-queue";   //nome della coda usata per inviare il contenuto del carrello al catalogo
        public const string CatalogToOrder = "ordercheckout-queue";   //nome della coda usata per inviare il contenuto del carrello al catalogo
    }
}
