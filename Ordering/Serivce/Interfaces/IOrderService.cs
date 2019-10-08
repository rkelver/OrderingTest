using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Models;
using Models.Interfaces;

namespace Serivce.Interfaces
{
    public interface IOrderService
    {
        OrderAdded Process(OrderRuleTypeEnum rule);
        void AddOrder(PendingOrder order);
    }
}
