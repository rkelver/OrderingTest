using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using CromulentBisgetti.ContainerPacking.Entities;
using Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Model.Interfaces;
using Models;
using Models.Interfaces;
using Queue;
using Queues;
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
        private IPackingService PackingService;
        private readonly CromulentBisgetti.ContainerPacking.Entities.Container Container;
        //6x6x4 => mm
        private decimal Length = 152.4m;
        private decimal Width = 152.4m;
        private decimal Height = 101.6m;

        public readonly ConcurrentDictionary<OrderRuleTypeEnum, IQueue<PendingOrder>> Factories;
        public OrderService()
        {
            Factories = new ConcurrentDictionary<OrderRuleTypeEnum, IQueue<PendingOrder>>();
            Container = new Container(int.MinValue,this.Length,this.Width, this.Height );

            SetUpDI();

            foreach (OrderRuleTypeEnum action in Enum.GetValues(typeof(OrderRuleTypeEnum)))
            {
                var idString = Type.GetType($"Queue.{action}`1, Queue");

                var type = idString.MakeGenericType(typeof(PendingOrder));
                IQueue<PendingOrder> ret = (IQueue<PendingOrder>)Activator.CreateInstance(type);
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
            var queue = Factories[rule];
            var order = queue.GetNext();

            var orderAdded = new OrderAdded
            {
                AddedAtDate = DateTime.UtcNow,
                CountryType = order.CountryType,
                FulFilled = order.Items.All(i=>i.FulFilled),
                Id = order.Id,
                Items = order.Items,
                LastTryDate = DateTime.UtcNow,
                OrderOriginationDate = order.OrderDate
            };

            return orderAdded;
        }

        private void AutoQueue_OrderAdded(IOrderAdded orderAdded)
        {
            Process(orderAdded.OrderRuleType);
            var leftOvers = orderAdded.Items?.Where(i => !i.FulFilled && i.Retries < AllowedItemRetryCount);
            if (leftOvers.Any())
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
                var items = orderAdded.Items?.Where(i => i.FulFilled);

                if (items != null && items.Any())
                {
                    var itemstoPack = new List<CromulentBisgetti.ContainerPacking.Entities.Item>();

                    foreach (var item in items)
                    {
                        itemstoPack.Add(new Item(int.MinValue, item.Dimensions.Length, item.Dimensions.Width,
                            item.Dimensions.Height, 1));
                    }

                    var packResults = PackingService.Pack(new Container(),itemstoPack);

                    foreach (var result in packResults)
                    {
                        //WE SHOULD LOG UNIQUE IDS of items moved to next box 
                        System.Diagnostics.Debug.WriteLine(result.AlgorithmPackingResults.First().IsCompletePack);
                    }
                }

        }

        private void SetUpDI()
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IDimensions ,Dimensions>()
                .AddTransient<IPackingService, PackingService>()
                .BuildServiceProvider();
            PackingService = serviceProvider
                .GetService<IPackingService>();
        }
    }
}
