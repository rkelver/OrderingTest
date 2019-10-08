using System;
using System.Collections.Generic;
using System.Text;
using Models.Interfaces;

namespace Models
{
    public partial class Item : IItem
    {
        public Item()
        {
            Dependencies = new List<Dependency>();
            FulFilled = true;
        }
        public Guid Id { get; set; }
        public Dimensions Dimensions { get; set; }
        public bool FulFilled { get; set; }
        public int Retries { get; set; }
        public List<Dependency> Dependencies { get; set; }
        public bool CanSend { get; set; }
    }
}
