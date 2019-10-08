using System.Collections.Generic;
using CromulentBisgetti.ContainerPacking.Entities;

namespace Serivce.Interfaces
{
    public interface IPackingService
    {
        List<ContainerPackingResult> Pack(Container container, List<Item> itemsToPack);
    }
}