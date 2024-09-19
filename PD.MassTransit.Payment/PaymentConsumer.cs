using MassTransit;
using PD.MassTransit.StateMachine;

namespace PD.MassTransit.Payment
{
    public class PaymentConsumer : IConsumer<PaymentProcessed>
    {
        public async Task Consume(ConsumeContext<PaymentProcessed> context)
        {
            // Simulate payment processing
            Console.WriteLine($"Processing payment for Order: {context.Message.OrderId}");

            //await context.Publish<PaymentProcessed>(new
            //{
            //    context.Message.OrderId,
            //    Timestamp = DateTime.UtcNow
            //});
        }
    }
}
