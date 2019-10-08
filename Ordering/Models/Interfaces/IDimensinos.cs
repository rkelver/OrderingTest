using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Interfaces
{
    public interface IDimensions
    {
        decimal Length { get; }
        decimal Width { get; }
        decimal Height { get; }
    }

}
