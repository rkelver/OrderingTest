using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Common;

namespace Models.Interfaces
{
    public interface IOrder
    {
        Guid Id { get; set; }
        List<Item> Items { get; set; }
        DateTime OrderOriginationDate { get; set; }
        CountryTypeEnum CountryType { get; set; }
        OrderRuleTypeEnum OrderRuleType { get; set; }
        DateTime OrderDate { get; set; }
        int BoxCount { get; set; }
    }
}
