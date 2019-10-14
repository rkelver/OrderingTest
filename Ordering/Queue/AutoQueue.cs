using System.Collections.Concurrent;
using Common;
using Models.Interfaces;
using Queue.Interfaces;

namespace Queue
{
    //TODO: THIS WILL GO TO A HARDWARE BASED QUEUE
    public class AutoQueue<T> : Queue<T>, IAutoQueue<T> where T : IPendingOrder, new()
    {
        private readonly Queue<T> ManualQueue = new ManualQueue<T>();

        public AutoQueue()
        {
            Orders = new ConcurrentBag<T>();
        }

        public override void Add(T order)
        {
            if (CanAutoQueue(order))
                base.Add(order);
            else
                //TODO: THERE IS NO REQUIREMENT IF IT SHOULD BE THE AUTO QUEUE TO MOVE TO MANUAL OR THE CALLER
                //SELF MANAGE QUEUE FOR NOW
                ManualQueue.Add(order);
        }

        public override bool CanAutoQueue(T item)
        {
            return item.CountryType == CountryTypeEnum.US;
        }
    }
}