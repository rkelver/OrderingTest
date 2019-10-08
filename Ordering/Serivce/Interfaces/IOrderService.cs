using Common;
using Models;

namespace Serivce.Interfaces
{
    public interface IOrderService
    {
        OrderAdded Process(OrderRuleTypeEnum rule);
        void AddOrder(PendingOrder order);
    }
}