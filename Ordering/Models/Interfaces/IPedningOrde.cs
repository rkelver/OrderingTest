using System;

namespace Models.Interfaces
{
    public interface IPendingOrder : IOrder
    {
        Guid OriginalOrderId { get; set; }
    }
}