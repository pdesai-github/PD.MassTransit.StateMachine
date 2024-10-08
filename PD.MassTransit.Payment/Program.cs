using MassTransit;
using PD.MassTransit.StateMachine;

namespace PD.MassTransit.Payment
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
                x.AddConsumer<PaymentConsumer>();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host("Endpoint=sb://pdazservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kMgsjvp/SElaBQZ/JyjelWap1uWimfXMA+ASbKQJMcI=");
                    cfg.Message<PaymentProcessed>(configTopology =>
                    {
                        configTopology.SetEntityName("ordertopic");  // This defines the topic name explicitly
                    });

                    cfg.Message<PaymentSuccessfull>(configTopology =>
                    {
                        configTopology.SetEntityName("ordertopic"); // Topic name
                    });

                    cfg.SubscriptionEndpoint<PaymentProcessed>("NotificationSubscription", endpointConfig =>
                    {
                        endpointConfig.ConfigureConsumer<PaymentConsumer>(context);
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
