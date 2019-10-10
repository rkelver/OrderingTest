using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Serivce;
using Serivce.Interfaces;

namespace Ordering
{
    internal class Program
    {
        private const int itemsToQueue = 10;
        private static List<PendingOrder> Orders;
        private static IOrderService OrderService;

        private static void Main(string[] args)
        {
            //SERVICES SHOULD USE DI TO USE OPEN CLOSE PRINCIPLE AND BE "MOCK-ABLE"
            //with more time this would allow much more of a single responsibility principle
            SetUpDI();

            //SHOULD BE IN A TEST INIT
            for (var i = 0; i < itemsToQueue; i++)
            {
                var isDependent = Convert.ToBoolean(i % 2);
                SetAutoQueueUpOrders(isDependent);
            }
        }

        private static void SetUpDI()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IOrderService, OrderService>()
                .AddTransient<IPackingService, PackingService>()
                .BuildServiceProvider();
            OrderService = serviceProvider
                .GetService<IOrderService>();
        }

        private static void SetAutoQueueUpOrders(bool isDependent)
        {
            Orders = new List<PendingOrder>();
            var random = new Random();
            var index = random.Next(Inventory.ItemIds.Count);
            Orders.Add(new PendingOrder
            {
                CountryType = CountryTypeEnum.US,
                Id = Guid.NewGuid(),
                Items = new List<Item>
                {
                    new Item
                    {
                        Id = Inventory.ItemIds[index],
                        Dimensions = new Dimensions(10,10,10)
                    }
                }
            });

            //BELONGS IN TEST
            if (isDependent)
            {
                var orderItems = Orders.FirstOrDefault()?.Items;

                if (orderItems != null && orderItems.Count > 1)
                    foreach (var item in orderItems.Where(i => i != orderItems.Last()))
                    {
                        var dependent = new Dependency();
                        dependent.Id = Guid.NewGuid();
                        dependent.Dependencies = (item.Id, orderItems.Last().Id);
                        Orders.Last().Items.Last().Dependencies = new List<Dependency> {dependent};
                    }
            }

            foreach (var order in Orders) OrderService.AddOrder(order);
        }
    }
}