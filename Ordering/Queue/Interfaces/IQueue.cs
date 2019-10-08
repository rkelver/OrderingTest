using System;
using System.Collections.Generic;
using System.Text;
using Exceptions;
using Models.Interfaces;

namespace Queue.Interfaces
{
    public interface IQueue<T>
    {
        event Action<IOrderAdded> OrderAdded;
        void Add(T order);
        void AddRange(IEnumerable<T> orders);

        T GetNext();
        void Remove(T order);

        void Move(T order);

        bool CanAutoQueue(T order);
    }
}
