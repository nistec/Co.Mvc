using Nistec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Pro.Mvc.Models
{
    //public class EntityListModel : GenericModel
    //{
    //    public EntityListModel() { }
    //    public EntityListModel(HttpRequestBase Request)
    //    {
    //        Load(Request);
    //    }
    //    public string ListJson { get; set; }

    //}

     public class EntityModel : GenericModel
    {
        public EntityModel() { }
        public EntityModel(HttpRequestBase Request)
        {
            Load(Request);
        }
        public JsonResult Data { get; set; }
        public string Content
        {
            get
            {
                if (Data == null)
                    return null;
                string json = Json.Encode(Data);
                return json;
            }
        }

    }

    public class GenericModel
    {
        public GenericModel() { }
        public GenericModel(HttpRequestBase Request)
        {
            Option = Request["op"];
            Id = Types.ToInt(Request["id"]);
            PId = Types.ToInt(Request["pid"]);
            Args = Request["args"];
        }

        protected void Load(HttpRequestBase Request)
        {
            Option = Request["op"];
            Id = Types.ToInt(Request["id"]);
            PId = Types.ToInt(Request["pid"]);
            Args = Request["args"];
        }

        public int AddId
        {
            get { return (Option == "a") ? Id : default(int); }
        }
        public int EditId
        {
            get { return (Option == "e") ? Id : default(int); }
        }

        //g-a-e-d
        public string Option { get; set; }
        public int Id { get; set; }
        public int PId { get; set; }
        public string Args { get; set; }

        public bool IsEdit
        {
            get { return Option.Contains("e"); }
        }
        public bool IsAdd
        {
            get { return Option.Contains("a"); }
        }
        public bool IsView
        {
            get { return Option.Contains("g"); }
        }
        public bool IsDelete
        {
            get { return Option.Contains("d"); }
        }
        public string IsEditClass
        {
            get { return IsEdit ? "item-pasive" : ""; }
        }
        public string IsAddClass
        {
            get { return IsAdd ? "item-pasive" : ""; }
        }
        public string IsViewClass
        {
            get { return IsView ? "item-pasive" : ""; }
        }
        public string IsDeleteClass
        {
            get { return IsDelete ? "item-pasive" : ""; }
        }
    }
}