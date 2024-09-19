using MassTransit;

namespace PD.MassTransit.StateMachine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(x =>
            {
                x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                    .InMemoryRepository();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host("Endpoint=sb://pdazservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kMgsjvp/SElaBQZ/JyjelWap1uWimfXMA+ASbKQJMcI=");
                    cfg.Message<OrderSubmitted>(x =>
                    {
                        x.SetEntityName("ordertopic"); // Set entity name explicitly
                    });

                    cfg.Message<PaymentProcessed>(x =>
                    {
                        x.SetEntityName("ordertopic"); // Set entity name explicitly
                    });
                    cfg.SubscriptionEndpoint<OrderSubmitted>("PaymentSubscription", endpointConfig =>
                    {
                        endpointConfig.ConfigureSaga<OrderState>(context);
                    });

                  

                    cfg.ConfigureEndpoints(context);
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
