using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Container
    {
        public IEnumerable<Item> Items { get; set; }
        public Dimensions Dimensions { get;}
        public decimal AvailableSpace { get; }

        public Container(Dimensions dimensions)
        {
            Dimensions = dimensions;
        }

        public Container(Dimensions dimensions,IEnumerable<Item> items)
        {
            Dimensions = dimensions;
            Items = items;
        }
    }
}
