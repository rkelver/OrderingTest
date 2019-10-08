using System;
using System.Collections.Generic;
using Common;
using Models.Interfaces;

namespace Models
{
    public class Order : IOrder
    {
        public Order()
        {
            Items = new List<Item>();
        }

        public Guid Id { get; set; }
        public List<Item> Items { get; set; }
        public DateTime OrderOriginationDate { get; set; }
        public CountryTypeEnum CountryType { get; set; }
        public OrderRuleTypeEnum OrderRuleType { get; set; }
        public DateTime OrderDate { get; set; }
        public int BoxCount { get; set; }
    }

}
