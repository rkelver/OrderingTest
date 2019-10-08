using System;

namespace Models.Interfaces
{
    public interface IOrderAdded : IOrder
    {
        DateTime AddedAtDate { get; set; }
        DateTime LastTryDate { get; set; }
        bool FulFilled { get; set; }
    }
}