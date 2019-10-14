using System;
using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public class Item : IItem
    {
        public Item()
        {
            Dependencies = new List<Dependency>();
            Dimensions = new Dimensions();
        }

        public Guid Id { get; set; }
        public Dimensions Dimensions { get; set; }
        public bool FulFilled { get; set; }
        public int Retries { get; set; }
        public List<Dependency> Dependencies { get; set; }
        public bool CanSend { get; set; }
    }
}