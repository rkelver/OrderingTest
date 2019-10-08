using System;
using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IItem
    {
        Guid Id { get; set; }
        Dimensions Dimensions { get; set; }
        bool FulFilled { get; set; }
        int Retries { get; set; }
        List<Dependency> Dependencies { get; set; }
        bool CanSend { get; set; }
    }
}