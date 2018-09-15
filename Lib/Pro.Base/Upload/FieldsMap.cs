using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Nistec.Data;
using System.Text.RegularExpressions;
using Nistec.Data.Entities;

namespace Pro.Upload
{
    public enum SourceTypes
    {
        Members=2,
        Contacts=3
    }

    public class FieldsMap : IEntityItem
    {
        public int AccountId { get; set; }
        public string Semantic { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public string EntityType { get; set; }
        public string FieldType { get; set; }
        public int FieldLength { get; set; }
        public int FieldOrder { get; set; }
        public string DefaultValue { get; set; }
        public bool Require { get; set; }
        public bool IsNullable { get; set; }
        [EntityProperty(EntityPropertyType.NA)]
        public string SourceField { get; set; }

    }

}
