using Nistec.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pro.Data.Entities.PropsEnum
{
    public interface IEntityEnum : IEntityProItem
    {
        int PropId { get; set; }
        string PropName { get; set; }
        string PropType { get; set; }
        int AccountId { get; set; }
        T Get<T>(int PropId, string PropType, int AccountId) where T : IEntityItem;
    }
}
