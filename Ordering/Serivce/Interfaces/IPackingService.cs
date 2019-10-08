using System;
using System.Collections.Generic;
using System.Text;
using CromulentBisgetti.ContainerPacking.Entities;

namespace Serivce.Interfaces
{
    public interface IPackingService
    {
        List<ContainerPackingResult> Pack(Container container, List<Item> itemsToPack);
    }
}
