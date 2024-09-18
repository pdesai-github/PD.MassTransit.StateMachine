using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.MassTransit.Shared
{
    public class PaymentProcessed
    {
        Guid CorrelationId { get; }
        Guid OrderId { get; }
        DateTime Timestamp { get; }
    }
}
