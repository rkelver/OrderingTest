using System;
using System.Collections.Generic;
using System.Text;
using Models.Interfaces;

namespace Models
{
    public partial class OrderAdded : Order, IOrderAdded
    {
        public Dimensions Dimensions { get; set; }
        public bool FulFilled { get; set; }
        public DateTime AddedAtDate { get; set; }
        public DateTime LastTryDate { get; set; }


        public OrderAdded()
        {

        }
    }
}
