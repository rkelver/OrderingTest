using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Exceptions;
using Models;
using Models.Interfaces;
using Queue.Interfaces;

namespace Queue
{
    //TODO: THIS WILL GO TO A HARDWARE BASED QUEUE
    public abstract class Queue<T> : IQueue<T> where T : IPendingOrder, new()
    {
        protected IProducerConsumerCollection<T> Orders;
        public event Action<IOrderAdded> OrderAdded = delegate { };

        public virtual void Add(T order)
        {
            if (!Orders.TryAdd(order))
                throw new OrderingAddItemException(order.Id, Orders.FirstOrDefault()?.OrderRuleType.ToString());

            var orderAdded = new OrderAdded
            {
                AddedAtDate = DateTime.UtcNow,
                CountryType = order.CountryType,
                FulFilled = false,
                Id = order.Id,
                Items = order.Items,
                LastTryDate = DateTime.UtcNow,
                OrderOriginationDate = order.OrderDate
            };

            OrderAdded(orderAdded);
        }

        public virtual void AddRange(IEnumerable<T> orders)
        {
            foreach (var order in orders) Add(order);
        }

        public virtual T GetNext()
        {
            return default;
        }

        public virtual void Remove(T item)
        {
        }

        public virtual void Move(T item)
        {
        }

        public virtual bool CanAutoQueue(T item)
        {
            return false;
        }

        public virtual T DoWork<T>(T order) where T : IPendingOrder, new()
        {
            var haveDependencies = order.Items.Where(i => i.Dependencies.All(d => !d.FulFilled));
            var leftOvers = Inventory.ItemsNotInInventory(order.Items.Select(i => i.Id));

            var retVal = new T
            {
                OriginalOrderId = order.Id,
                Id = Guid.NewGuid()
            };

            //SHOULD EXTEND ADDRANGE to mark fulfiled = false
            retVal.Items.AddRange(leftOvers);
            retVal.Items.AddRange(haveDependencies);

            foreach (var item in order.Items.Where(o=> !retVal.Items.Contains(o))) item.FulFilled = true;

            return retVal;
        }
    }
}