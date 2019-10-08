using System;
using System.Collections.Generic;
using System.Linq;
using CromulentBisgetti.ContainerPacking.Algorithms;
using CromulentBisgetti.ContainerPacking.Entities;
using Model.Interfaces;
using Serivce.Interfaces;

namespace Serivce
{
    public class PackingService : IPackingService
    {
        private readonly object sync = new object();

        public PackingService()
        {
        }

        public PackingService(IDimensions dimensions, List<Item> itemsToPack)
        {
            Pack(new Container(int.MinValue, dimensions.Length, dimensions.Width, dimensions.Height), itemsToPack);
        }

        public List<ContainerPackingResult> Pack(Container container, List<Item> itemsToPack)
        {
            var result = new List<ContainerPackingResult>();
            var containerPackingResult = new ContainerPackingResult();
            containerPackingResult.ContainerID = container.ID;
            var algorithm = GetPackingAlgorithmFromTypeID(1); //magic # // only enum avail in this 3rd party tool

            // Until I rewrite the algorithm with no side effects, we need to clone the item list
            // so the parallel updates don't interfere with each other.
            var items = new List<Item>();

            itemsToPack.ForEach(item =>
            {
                items.Add(new Item(item.ID, item.Dim1, item.Dim2, item.Dim3, item.Quantity));
            });

            var algorithmResult = algorithm.Run(container, items);

            var containerVolume = container.Length * container.Width * container.Height;
            var itemVolumePacked = algorithmResult.PackedItems.Sum(i => i.Volume);
            var itemVolumeUnpacked = algorithmResult.UnpackedItems.Sum(i => i.Volume);

            algorithmResult.PercentContainerVolumePacked = Math.Round(itemVolumePacked / containerVolume * 100, 2);
            algorithmResult.PercentItemVolumePacked =
                Math.Round(itemVolumePacked / (itemVolumePacked + itemVolumeUnpacked) * 100, 2);

            lock (sync)
            {
                containerPackingResult.AlgorithmPackingResults.Add(algorithmResult);
            }

            return result;
        }

        public static IPackingAlgorithm GetPackingAlgorithmFromTypeID(int algorithmTypeID)
        {
            switch (algorithmTypeID)
            {
                case (int) AlgorithmType.EB_AFIT:
                    return new EB_AFIT();

                default:
                    throw new Exception("Invalid algorithm type.");
            }
        }
    }
}