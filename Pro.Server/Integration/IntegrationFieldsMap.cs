using Nistec.Data.Entities;
using Nistec.Data;
using Pro.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Nistec;
using System.Text.RegularExpressions;
using Pro.Upload;

namespace Pro.Server.Integration
{


    public class IntegrationFieldsMap : IntegrationFields
    {
        //public int SourceType { get; set; }
        //public string FieldName { get; set; }
        //public string SourceName { get; set; }
        //public string Semantic { get; set; }
        //public int FieldOrder { get; set; }
        //public string DefaultValue { get; set; }
        //public string SourceFieldMap { get; set; }
        //public string SourceFormat { get; set; }
        //public string SourceFilter { get; set; }
        //public string SourceFilterType { get; set; }
        //public string FieldType { get; set; }
        //public bool IsRuleField { get; set; }
               

        public static object GetDefaultValue(string fieldType, string defaultValue)
        {
            Type type = GetFieldType(fieldType);
            if (defaultValue == null)
            {
                if (type.IsValueType)
                    return GenericTypes.Default(type);
                return null;
            }
            if (defaultValue == "default" || defaultValue == "NA")
                return GenericTypes.Default(type);

            return Types.ChangeType(defaultValue, type);
        }
        public static Type GetFieldType(string fieldType)
        {
            switch (fieldType)
            {
                case "text":
                case "sdate":
                case "cli":
                case "phone":
                case "mail":
                case "sex":
                case "ukey":
                    return typeof(string);
                case "bool":
                    return typeof(bool);
                case "acc":
                case "state":
                case "rule":
                case "combo":
                case "int":
                case "auto":
                    return typeof(int);
                //case "now":
                case "date":
                    return typeof(DateTime);
                case "guid":
                    return typeof(Guid);
                default:
                    return typeof(object);
            }
        }

        public object GetValue(object val)
        {



            return null;
        }

        object DoSourceMap(object val)
        {
            if (SourceFieldMap == null)
                return val;

            return val;
        }

        public object DoSourceFormat(object val)
        {
            if (SourceFormat == null)
                return val;

            switch (FieldType)
            {
                case "text":
                    return val;
                case "sdate":
                    //string value = "Jan 06 2015";//"January 06, 2015";// "10/15/16";// "10/15/2016";// "2016-10-15";
                    //string format = "MMM dd yyyy";//"MMMM dd, yyyy";// "MM/dd/yy";// "MM/dd/yyyy";// "yyyy-MM-dd";
                    return Types.DateFormatExact(val, SourceFormat, "dd/MM/yyyy");
                case "cli":
                    if (val == null)
                        return DefaultValue;
                    return UploadReader.EnsureLocalCli(val.ToString(), DefaultValue);
                case "phone":
                    if (val == null)
                        return DefaultValue;
                    return UploadReader.EnsureLocalCli(val.ToString(), DefaultValue);
                case "mail":
                    return UploadReader.EnsureEmail(Types.NZ(val, DefaultValue), DefaultValue);
                case "sex":
                    return UploadReader.ReadSexField(val, "U");
                case "ukey":
                    return typeof(string);
                case "bool":
                    return Types.ChangeType(val, typeof(bool));
                case "acc":
                case "state":
                case "rule":
                case "combo":
                case "int":
                case "auto":
                    return typeof(int);
                //case "now":
                case "date":
                    return Types.DateFormatExact(val, SourceFormat);
                case "guid":
                    return typeof(Guid);
                default:
                    return val;
            }

        }

        public bool ValidateFilter(object val)
        {
            if (SourceFilter == null || SourceFilterType == null)
                return false;
            string filter = SourceFilter.ToString().Trim();
            string content = Types.NZ(val, "").Trim();

            switch (SourceFilterType)
            {
                case "none":
                    return false;
                case "empty":
                    return (content == "");
                case "in":
                    if (Regex.IsMatch(content, @"\b(" + filter.Replace(",", "|") + ")\b", RegexOptions.IgnoreCase))
                        return true;
                    else
                        return false;
                case "=":
                    if (Regex.IsMatch(content, @"\b" + filter + "\b", RegexOptions.IgnoreCase))
                        return true;
                    else
                        return false;
                case ">=":
                    return Types.ToDouble(content, 0) >= Types.ToDouble(filter, 0);
                case "<=":
                    return Types.ToDouble(content, 0) <= Types.ToDouble(filter, 0);

            }

            return false;
        }

    }

}
