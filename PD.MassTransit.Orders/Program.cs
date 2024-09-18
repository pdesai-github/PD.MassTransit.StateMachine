using MassTransit;
using PD.MassTransit.StateMachine;

namespace PD.MassTransit.Orders
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
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host("Endpoint=sb://pdazservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=kMgsjvp/SElaBQZ/JyjelWap1uWimfXMA+ASbKQJMcI=");

                    cfg.Message<OrderSubmitted>(configTopology =>
                    {
                        configTopology.SetEntityName("ordertopic"); // Topic name
                    });

                    cfg.Message<PaymentProcessed>(configTopology =>
                    {
                        configTopology.SetEntityName("ordertopic"); // Topic name
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
