using System;
using Models.Interfaces;

namespace Models
{
    public class OrderAdded : Order, IOrderAdded
    {
        public Dimensions Dimensions { get; set; }
        public bool FulFilled { get; set; }
        public DateTime AddedAtDate { get; set; }
        public DateTime LastTryDate { get; set; }
    }
}