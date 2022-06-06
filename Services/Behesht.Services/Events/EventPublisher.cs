using Behesht.Services.Events;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Behesht.Services.Events
{
    /// <summary>
    /// Represents the event publisher implementation
    /// </summary>
    public partial class EventPublisher : IEventPublisher
    {

        private readonly IServiceProvider _serviceProvider;

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="serviceProvider">service provider</param>
        public EventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Publish event to consumers
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="event">Event object</param>
        public virtual async Task PublishAsync<TEvent>(TEvent @event)
        {
            //get all event consumers
            var consumers = _serviceProvider.GetServices<IConsumer<TEvent>>();

            foreach (var consumer in consumers)
            {
                try
                {
                    //try to handle published event
                    await consumer.HandleEventAsync(@event);
                }
                catch (Exception exception)
                {
                    try
                    {
                        //TODO: add required logs
                    }
                    catch { }
                }
            }
        }

        #endregion
    }
}