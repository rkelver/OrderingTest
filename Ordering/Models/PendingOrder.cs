using System;
using Models.Interfaces;

namespace Models
{
    public class PendingOrder : Order, IPendingOrder
    {
        public Guid OriginalOrderId { get; set; }
    }
}