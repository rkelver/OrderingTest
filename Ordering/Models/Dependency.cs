using System;
using Models.Interfaces;

namespace Models
{
    public class Dependency : IDependency
    {
        public bool FulFilled { get; set; }
        public Guid Id { get; set; }
        public (Guid Id1, Guid Id2) Dependencies { get; set; }
    }
}