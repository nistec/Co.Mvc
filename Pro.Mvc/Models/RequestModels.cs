using Nistec.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Pro.Mvc.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class EditTaskModel : EditModel<int,int>
    {
        public EditTaskModel() { }
        public EditTaskModel(HttpRequestBase Request) { Load(Request); }
    }
    public class EditModel : EditModel<int,int>
    {
        public EditModel() { }
        public EditModel(HttpRequestBase Request) { Load(Request); }
   }
    public class EditModel<Tp, Ti>
        where Tp : struct
        where Ti : struct
    {
        public EditModel() { }
        public EditModel(HttpRequestBase Request)
        {
            Option = Request["op"];
            Id = Nistec.GenericTypes.ConvertTo<Ti>(Request["id"]);//.Types.ToLong(Request["id"]);
            PId = Nistec.GenericTypes.ConvertTo<Tp>(Request["pid"]);
            Args = Request["args"];
        }

        protected void Load(HttpRequestBase Request)
        {
            Option = Request["op"];
            Id = Nistec.GenericTypes.ConvertTo<Ti>(Request["id"]);//.Types.ToLong(Request["id"]);
            PId = Nistec.GenericTypes.ConvertTo<Tp>(Request["pid"]);
            Args = Request["args"];
        }

        public Ti AddId
        {
            get { return (Option == "a") ? Id : default(Ti); }
        }
        public Ti EditId
        {
            get { return (Option == "e") ? Id : default(Ti); }
        }

        //g-a-e-d
        public string Option { get; set; }
        public Ti Id { get; set; }
        public Tp PId { get; set; }
        public string Args { get; set; }
        public object Data { get; set; }

        public bool IsEdit
        {
            get { return Option==null ? false:Option.Contains("e"); }
        }
        public bool IsAdd
        {
            get { return Option == null ? false : Option.Contains("a"); }
        }
        public bool IsView
        {
            get { return Option == null ? false : Option.Contains("g"); }
        }
        public bool IsDelete
        {
            get { return Option == null ? false : Option.Contains("d"); }
        }
        public string IsEditClass
        {
            get { return IsEdit ? "item-pasive":""; }
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

    public class UploadProcModel
    {
        public string UploadKey { get; set; }
    }

    public class ViewModel
    {
        public ViewModel() { }
        public ViewModel(HttpRequestBase Request)
        {
            string viewType = Request["v"];
            ViewType = viewType;
            Args = Request["args"];
            Title = ViewModel.GetTitle(viewType);
        }

        public string ViewType { get; set; }
        public string Title { get; set; }
        public string Args { get; set; }
        public string ScriptSrc
        {
            get { return "/Scripts/app/view/" + ViewType + ".js"; }
        }

        public static string GetTitle(string viewType)
        {
            switch (viewType)
            {
                case "unpayed-grid":
                    return "פירוט נרשמים ללא תשלום";
                default:
                    return "דוח";
            }
        }

        //public class Feedback
        //{
        //    public int Id { get; set; }
        //    public string Title { get; set; }
        //    public string Comment { get; set; }

        //}  
    }
}
