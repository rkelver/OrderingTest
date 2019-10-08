using System;
using System.Collections.Generic;
using System.Text;
using Models.Interfaces;

namespace Models
{
    public class Dependency : IDependency
    {
        public Guid Id { get; set; }
        public (Guid Id1, Guid Id2) Dependencies { get; set; }
        public bool FulFilled { get; set; }
    }
}
