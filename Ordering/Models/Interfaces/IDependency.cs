using System;

namespace Models.Interfaces
{
    public interface IDependency
    {
        Guid Id { get; set; }
        (Guid Id1, Guid Id2) Dependencies { get; set; }
    }
}