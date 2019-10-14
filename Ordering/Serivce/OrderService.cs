using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Models;
using Models.Interfaces;
using Queue.Interfaces;
using Serivce.Interfaces;
using Container = CromulentBisgetti.ContainerPacking.Entities.Container;
using Item = CromulentBisgetti.ContainerPacking.Entities.Item;

namespace Serivce
{
    public class OrderService : IOrderService
    {
        //ALSO CAN BE SOLVED W a LEGIT QUEUE // dead letter
        private const int AllowedItemRetryCount = 3;

        public readonly ConcurrentDictionary<OrderRuleTypeEnum, IQueue<PendingOrder>> Factories;
        private IPackingService PackingService;
        //6x6x4 => mm
        private readonly decimal Length = 152.4m;
        private readonly decimal Height = 101.6m;
        private readonly decimal Width = 152.4m;

        public OrderService(IPackingService packingService)
        {
            Factories = new ConcurrentDictionary<OrderRuleTypeEnum, IQueue<PendingOrder>>();
            PackingService = packingService;
         
            foreach (OrderRuleTypeEnum action in Enum.GetValues(typeof(OrderRuleTypeEnum)))
            {
                var idString = Type.GetType($"Queue.{action}`1, Queue");

                var type = idString.MakeGenericType(typeof(PendingOrder));
                var ret = (IQueue<PendingOrder>) Activator.CreateInstance(type);
                ret.OrderAdded += AutoQueue_OrderAdded;
                Factories.TryAdd(action, ret);
            }
        }

        public OrderAdded Process(OrderRuleTypeEnum rule)
        {
            return GetNext(rule);
        }

        public void AddOrder(PendingOrder order)
        {
            var queue = GetRuleBasedQueue(order.OrderRuleType);
            queue.Add(order);
        }

        private IQueue<PendingOrder> GetRuleBasedQueue(OrderRuleTypeEnum orderOrderRuleType)
        {
            return Factories[orderOrderRuleType];
        }

        private OrderAdded GetNext(OrderRuleTypeEnum rule)
        {
            var queue = GetRuleBasedQueue(rule);
            var order = queue.GetNext();

            var orderAdded = new OrderAdded
            {
                AddedAtDate = DateTime.UtcNow,
                CountryType = order.CountryType,
                FulFilled = order.Items.All(i => i.FulFilled),
                Id = order.Id,
                Items = order.Items,
                LastTryDate = DateTime.UtcNow,
                OrderOriginationDate = order.OrderDate
            };

            return orderAdded;
        }

        private void AutoQueue_OrderAdded(IOrderAdded orderAdded)
        {
            orderAdded = Process(orderAdded.OrderRuleType);

            var leftOvers = orderAdded.Items?.Where(i => !i.FulFilled && i.Retries < AllowedItemRetryCount).ToList();
            if (leftOvers != null && leftOvers.Any())
            {
                //REQUEUE WITH NON FULFILLED ITEMS
                var requeueOrder = new PendingOrder
                {
                    Id = Guid.NewGuid(),
                    OriginalOrderId = orderAdded.Id,
                    CountryType = orderAdded.CountryType
                };

                foreach (var item in leftOvers)
                {
                    item.FulFilled = false;
                    item.Retries++;
                    item.Dependencies = item.Dependencies;
                }

                GetRuleBasedQueue(orderAdded.OrderRuleType).Add(requeueOrder);
            }


            //write out logistics/boxes....there is no requirements on what to do based on volume available....
            //ASSUMPTION USING mm
            var items = orderAdded.Items?.Where(i => i.FulFilled).ToList();

            if (items != null && items.Any())
            {
                var itemsToPack = new List<Item>();

                foreach (var item in items)
                    itemsToPack.Add(new Item(int.MinValue, item.Dimensions.Length, item.Dimensions.Width,
                        item.Dimensions.Height, items.Count()));

                var packResults = PackingService.Pack(new Container(int.MinValue,Length,Width,Height), itemsToPack);

                foreach (var result in packResults)
                    //WE SHOULD LOG UNIQUE IDS of items moved to next box 
                    Debug.WriteLine(result.AlgorithmPackingResults.First().IsCompletePack);
            }
        }
    }
}