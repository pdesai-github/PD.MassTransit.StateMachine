using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.MassTransit.Shared
{
    public class OrderSubmitted
    {
        Guid CorrelationId { get; }
        Guid OrderId { get; }
        Guid ProductId { get; }
        int Quantity { get; }
        DateTime Timestamp { get; }
    }
}
