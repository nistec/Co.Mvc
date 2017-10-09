using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Pro.Mvc.Models
{


    //public class UserModel : Nistec.Web.Security.SignedUser
    //{
    //    //public int UserId { get; set; }
    //    //public string UserName { get; set; }
    //    //public string DisplayName { get; set; }
    //    //public int UserRole { get; set; }
    //    //public int AccountId { get; set; }
    //    //public string AccountName { get; set; }
    //    //public int AccountCategory { get; set; }
    //    //public int ParentId { get; set; }
    //    //public bool IsMobile { get; set; }
        
    //    //v-e-r-x-m
    //    //view-edit-remove-export-management
    //    public bool AllowEdit
    //    {
    //        get { return UserRole >= 1; }
    //    }
    //    public bool AllowAdd
    //    {
    //        get { return UserRole >= 1; }
    //    }
    //    public bool AllowExport
    //    {
    //        get { return UserRole >= 5; }
    //    }
    //    public bool AllowDelete
    //    {
    //        get { return UserRole >= 5; }
    //    }
    //    public string AllowEditClass
    //    {
    //        get { return AllowEdit ? "" : "item-pasive"; }
    //    }
    //    public string AllowAddClass
    //    {
    //        get { return AllowAdd ? "" : "item-pasive"; }
    //    }
    //    public string AllowExportClass
    //    {
    //        get { return AllowExport ? "" : "item-pasive"; }
    //    }
    //    public string AllowDeleteClass
    //    {
    //        get { return AllowDelete ? "" : "item-pasive"; }
    //    }
    //    public bool IsManager
    //    {
    //        get { return UserRole >= 5; }
    //    }
    //}

    //public class SignupModel
    //{
    //  public int AccountId { get; set; }
    //  public string Title { get; set; }
    //  public string Body { get; set; }

    //}

 
    public static class JavaScriptConvert
    {
        public static IHtmlString SerializeObject(object value)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var serializer = new JsonSerializer
                {
                    // Let's use camelCasing as is common practice in JavaScript
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                // We don't want quotes around object names
                jsonWriter.QuoteName = false;
                serializer.Serialize(jsonWriter, value);

                return new HtmlString(stringWriter.ToString());
            }
        }
    }
}