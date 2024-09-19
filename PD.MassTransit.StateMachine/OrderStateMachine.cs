using MassTransit;

namespace PD.MassTransit.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }
        public State Paid { get; private set; }

        public Event<OrderSubmitted> OrderSubmittedEvent { get; private set; }
        public Event<PaymentProcessed> PaymentProcessed { get; private set; }

        public Event<PaymentSuccessfull> PaymentSuccessfullEvent { get; private set; }

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
            Event(() => PaymentSuccessfullEvent, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            });

            Initially(
                When(OrderSubmittedEvent)
                    .Then(context =>
                    {
                        context.Instance.OrderId = context.Data.OrderId;
                        context.Instance.Created = context.Data.Timestamp;
                        context.Instance.CurrentState = "Submitted"; // Change state to "Submitted"

                    }).
                    Publish(context =>
                    {
                        return new PaymentProcessed
                        {
                            CorrelationId = context.Data.CorrelationId,
                            OrderId = context.Data.OrderId,
                            Timestamp = context.Data.Timestamp
                        };
                    })
                    .TransitionTo(Submitted)

            );

            During(Submitted,
                When(PaymentSuccessfullEvent)
                    .Then(context =>
                    {
                        context.Instance.OrderId = context.Data.OrderId;
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
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class PaymentSuccessfull
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime Timestamp { get; set; }

    }
}


