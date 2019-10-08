using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Models.Interfaces
{
    public interface IPendingOrder : IOrder
    {
        Guid OriginalOrderId { get; set; }
    }
}
