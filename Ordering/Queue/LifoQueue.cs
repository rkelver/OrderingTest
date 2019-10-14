using System.Collections.Concurrent;
using Exceptions;
using Models.Interfaces;
using Queue.Interfaces;

namespace Queue
{
    //TODO: THIS WILL GO TO A HARDWARE BASED QUEUE
    public class LifoQueue<T> : AutoQueue<T>, ILifoQueue<T> where T : IPendingOrder, new()
    {
        public LifoQueue()
        {
            Orders = new ConcurrentStack<T>();
        }

        public override T GetNext()
        {
            if (!Orders.TryTake(out var order)) throw new OrderingGetNextItemException(order.Id, order.OrderRuleType);

            return base.DoWork(order);
        }

        //LEFT TO SHOW RULES CAN BE ADDED TO AUTOQUEUE THAT ARE LIFO SPECIFIC // OTHERWISE PASS THROUGH
        public override bool CanAutoQueue(T item)
        {
            return base.CanAutoQueue(item);
        }
    }
}