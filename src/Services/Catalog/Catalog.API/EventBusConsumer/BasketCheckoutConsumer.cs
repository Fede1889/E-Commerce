using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Catalog.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        private readonly IProductRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint; //serve per inviare i messaggi alla coda

        public BasketCheckoutConsumer( IMapper mapper, ILogger<BasketCheckoutConsumer> logger, IProductRepository repository, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            CheckOut products = _mapper.Map<CheckOut>(context.Message);
            bool errore = false;
            string flag = "";
            // controllo se la quantita di ogni articolo e' sufficiente per coprire l'ordine
            foreach(var product in products.Items)
            {
                var app = await _repository.GetProduct(product.ProductId);
                if(app.Quantity < product.Quantity)
                {
                    errore = true;
                    flag = app.Id;
                    break;
                }

                if (app.Quantity >= product.Quantity)
                {
                    app.Quantity -= product.Quantity;
                    await _repository.UpdateProduct(app);
                }

            }

            // invio messaggio ad Ordering
            if (!errore)
            {
                _logger.LogInformation("Order checked: quantity ok");

                Order lista = new Order();
                lista.UserName = products.UserName;
                lista.TotalPrice = products.TotalPrice;

                // inviare l'oggetto che rappresenta la lista di oggetti alla coda
                var eventMessage = _mapper.Map<OrderEvent>(lista);
                await _publishEndpoint.Publish<OrderEvent>(eventMessage);
            }
            else
            {
                _logger.LogInformation("Order checked: not enought quantity for: ", flag);
            }
            
        }
    }
}
