using MassTransit;

namespace PD.MassTransit.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }
        public State Paid { get; private set; }

        public Event<OrderSubmitted> OrderSubmittedEvent { get; private set; }
        public Event<PaymentProcessed> PaymentProcessed { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderSubmittedEvent, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            });
            Event(() => PaymentProcessed, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            });

            Initially(
                When(OrderSubmittedEvent)
                    .Then(context =>
                    {
                        context.Instance.OrderId = context.Data.OrderId;
                        context.Instance.Created = context.Data.Timestamp;
                    })
                    .TransitionTo(Submitted)
            );

            During(Submitted,
                When(PaymentProcessed)
                    .Then(context =>
                    {
                        context.Instance.Created = context.Data.Timestamp;
                    })
                    .TransitionTo(Paid)
            );

        }
    }

    public class OrderSubmitted
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public DateTime Timestamp { get; private set; }
    }

    public class PaymentProcessed
    {
        public Guid CorrelationId { get; }
        public Guid OrderId { get; }
        public DateTime Timestamp { get; }
    }
}


