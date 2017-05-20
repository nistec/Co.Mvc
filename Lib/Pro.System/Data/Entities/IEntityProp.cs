using Nistec.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProSystem.Data.Entities
{
  
    public interface IEntityPro : IEntityProItem
    {
        int PropId { get; set; }
        string PropName { get; set; }
        int AccountId { get; set; }
        T Get<T>(int PropId) where T : IEntityItem;

    }

    public interface IEntityProEnum : IEntityPro
    {
        int EnumType { get; set; }
 
    }

    public interface IEntityProItem : IEntityItem
    {
        string MappingName();

        int DoUpdate<T>(T newEntity) where T : IEntityItem;

        int DoInsert<T>() where T : IEntityItem;

        int DoDelete<T>() where T : IEntityItem;
    }
}
