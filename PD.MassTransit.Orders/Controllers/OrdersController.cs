using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PD.MassTransit.StateMachine;

namespace PD.MassTransit.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IPublishEndpoint publishEndpoint;

        public OrdersController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public IActionResult Post(OrderSubmitted orderSubmitted)
        {
            orderSubmitted.CorrelationId = Guid.NewGuid();
            publishEndpoint.Publish(orderSubmitted);
            return Ok();
        }
    }
}
