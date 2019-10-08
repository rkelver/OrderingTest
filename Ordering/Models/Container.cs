using System.Collections.Generic;

namespace Models
{
    public class Container
    {
        public Container(Dimensions dimensions)
        {
            Dimensions = dimensions;
        }

        public Container(Dimensions dimensions, IEnumerable<Item> items)
        {
            Dimensions = dimensions;
            Items = items;
        }

        public IEnumerable<Item> Items { get; set; }
        public Dimensions Dimensions { get; }
        public decimal AvailableSpace { get; }
    }
}