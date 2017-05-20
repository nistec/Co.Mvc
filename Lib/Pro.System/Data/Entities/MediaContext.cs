using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using ProSystem.Data;
using Nistec.Data;
using Nistec.Web.Controls;
using System.IO;

namespace ProSystem.Data.Entities
{
    /*
    [Entity(EntityName = "MediaView", MappingName = "Crm_Media", ConnectionKey = "cnn_natam", EntityKey = new string[] { "MediaId" })]
    public class MediaContext : EntityContext<MediaView>
    {
        #region ctor

        public MediaContext()
        {
        }

        public MediaContext(int MediaId)
            : base(MediaId)
        {
        }

        #endregion

        #region update

        public static int DoSave(int id, MediaView bv, UpdateCommandType commandType)
        {
            if (commandType == UpdateCommandType.Delete)
                using (MediaContext context = new MediaContext(id))
                {
                    return context.SaveChanges(commandType);
                }

            EntityValidator.Validate(bv, "מדיה", "he");

            if (commandType == UpdateCommandType.Insert)
                using (MediaContext context = new MediaContext())
                {
                    context.Set(bv);
                    return context.SaveChanges(commandType);
                }

            if (commandType == UpdateCommandType.Update)
                using (MediaContext context = new MediaContext(id))
                {
                    context.Set(bv);
                    return context.SaveChanges(commandType);
                }
            return 0;
        }

        public static int DoSave(int id, MediaView bv)
        {
            EntityValidator.Validate(bv, "מדיה", "he");
            
            using (MediaContext context = new MediaContext())
            {
                UpdateCommandType cmdtype = UpdateCommandType.Insert;
                if (id > 0)
                {
                    context.SetEntity(id);
                    cmdtype = UpdateCommandType.Update;
                }
                context.Set(bv);
                return context.SaveChanges(cmdtype);
            }
        }

        public static int DoInsert(MediaView bv)
        {
            EntityValidator.Validate(bv, "מדיה", "he");

            using (MediaContext context = new MediaContext())
            {
                UpdateCommandType cmdtype = UpdateCommandType.Insert;
                context.Set(bv);
                return context.SaveChanges(cmdtype);
            }
        }

        public static int DoRemove(int id)
        {
            return DbNatam.Instance.ExecuteCommand("delete from Crm_Media where MediaId=@MediaId", System.Data.CommandType.Text, "MediaId", id);
        }

        #endregion

        #region static

        public static MediaView Get(int id)
        {
            using (MediaContext context = new MediaContext(id))
            {
                return context.Entity;
            }
        }

        public static string LookupByBuilding(int BuildingId)
        {
            string PropertyType = "b";
            return DbNatam.Instance.QueryScalar<string>("select top 1 MediaPath from Crm_Media where BuildingId=@BuildingId and PropertyType=@PropertyType", "", "BuildingId", BuildingId, "PropertyType", PropertyType);
        }
        public static string LookupByUnit(int UnitId)
        {
            string PropertyType = "u";
            return DbNatam.Instance.QueryScalar<string>("select top 1 MediaPath from Crm_Media where UnitId=@UnitId and PropertyType=@PropertyType", "", "UnitId", UnitId, "PropertyType", PropertyType);
        }
        public static string LookupByPlots(int PlotsId)
        {
            string PropertyType = "p";
            return DbNatam.Instance.QueryScalar<string>("select top 1 MediaPath from Crm_Media where BuildingId=@BuildingId and PropertyType=@PropertyType", "", "BuildingId", PlotsId, "PropertyType", PropertyType);
        }

        public static bool MediaPropertyExists(int propertyId)
        {
            var res = ViewByProperty(propertyId);
            return res != null && res.Count() > 0;
        }
        public static bool MediaBuildingExists(int buildingId)
        {
            var res = ViewByBuilding(buildingId);
            return res != null && res.Count() > 0;
        }
        public static bool MediaPlotsExists(int buildingId)
        {
            var res = ViewByPlots(buildingId);
            return res != null && res.Count() > 0;
        }
        public static IEnumerable<MediaView> View(int buildingId, int propertyId, string propertyType)
        {
            if (propertyType == "u")
                return ViewByProperty(propertyId);
            if (propertyType == "b")
                return ViewByBuilding(buildingId);
            if (propertyType == "p")
                return ViewByPlots(propertyId);
            throw new ArgumentException("propertyType not supported " + propertyType);
        }
        public static IEnumerable<MediaView> ViewByBuilding(int BuildingId)
        {
            string PropertyType = "b";
            using (MediaContext context = new MediaContext())
            {
                return context.EntityList(DataFilter.GetSql("BuildingId=@BuildingId and PropertyType=@PropertyType", BuildingId, PropertyType));
            }
        }
        public static IEnumerable<MediaView> ViewByProperty(int UnitId)
        {
            using (MediaContext context = new MediaContext())
            {
                return context.EntityList(DataFilter.GetSql("UnitId=@UnitId", UnitId));
            }
        }

        public static IEnumerable<MediaView> ViewByPlots(int PlotsId)
        {
            string PropertyType = "p";
            using (MediaContext context = new MediaContext())
            {
                return context.EntityList(DataFilter.GetSql("BuildingId=@BuildingId and PropertyType=@PropertyType", PlotsId, PropertyType));
            }
        }
        #endregion

    }

    public class MediaView : IEntityItem
    {

        #region properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int MediaId { get; set; }
        
        public int BuildingId { get; set; }
        
        public int UnitId { get; set; }
        
        public string PropertyType { get; set; }
        
        public string MediaType { get; set; }

        
        [Validator("נתיב מדיה",true)]
        public string MediaPath { get; set; }

        #endregion

    }
    */


    //[EntityMapping("Media_Files", "vw_Media_Files")]
    public class MediaContext : EntityContext<DbSystem, MediaFile>
    {
        public MediaContext(int userId)
        {
            CacheKey = DbContextCache.GetKey<MediaFile>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);
        }
        public static IList<MediaFile> GetMediaList(string RefId, MediaRefTypes refType, int pid, int userId)
        {
            int ttl = 3;
            string key = DbContextCache.GetKey<MediaFile>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);

            switch (refType)
            {
                case MediaRefTypes.Task:
                    return DbContextCache.EntityList<DbSystem, MediaFile>(key, ttl, new object[]{ "RefId", RefId});
                default:
                    return null;
            }
        }

        //public static IList<MediaFile> GetTaskList(int taskId, int pid, int userId)
        //{
        //    int ttl = 3;
        //    string key = DbContextCache.GetKey<MediaFile>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);
        //    return DbContextCache.EntityList<DbSystem, MediaFile>(key, ttl, "RefId", taskId.ToString());
        //}

        protected override void OnChanged(UpdateCommandType commandType)
        {
            DbContextCache.Remove(CacheKey);
        }

        public static string GetRefType(MediaRefTypes t)
        {
            switch (t)
            {
                case MediaRefTypes.Task:
                    return "t";
                case MediaRefTypes.Project:
                    return "p";
                case MediaRefTypes.Leads:
                    return "l";
            }
            return "t";
        }
        public static MediaRefTypes GetRefType(string t)
        {
            switch (t)
            {
                case "t":
                    return MediaRefTypes.Task;
                case "p":
                    return MediaRefTypes.Project;
                case "l":
                    return MediaRefTypes.Leads;
            }
            return  MediaRefTypes.Task;
        }
    }
    /*
    public class MediaContext2 : MediaFile
    {
        public static IList<MediaFile> GetList(int refId, int pid, int userId, int ttl)
        {
            string key = DbContextCache.GetKey<MediaFile>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);
            return DbContextCache.EntityList<DbSystem, MediaFile>(key, ttl, "RefId", refId);
        }
        public static MediaFile Get(int RefId, string FileName)
        {
            if (!string.IsNullOrEmpty(RefId) && !string.IsNullOrEmpty(FileName))
                return new MediaFile() { RefId = RefId };

            return DbContext.EntityGet<DbSystem, MediaFile>("FileName", FileName);
        }
        public static int Save(string RefId, string FileName, MediaFile entity, UpdateCommandType commandType)
        {
            return DbContext.EntitySave<DbSystem, MediaFile>(entity, commandType, new object[] { "RefId", RefId, "FileName", FileName });
        }
        public static int Add(MediaFile entity)
        {
            return DbContext.EntityInsert<DbSystem, MediaFile>(entity);
        }
        public static int Delete(string FileName)
        {
            return DbContext.EntityDelete<DbSystem, MediaFile>("FileName", FileName);
        }

    }
    */



    [EntityMapping("Media_Files","vw_Media_Files",ProcUpdate="sp_Media_Files_Update")]
    public class MediaFile : IEntityItem
    {
        [EntityProperty(EntityPropertyType.NA)]
        public string FileFullPath
        {
            get { return Path.Combine(FilePath,FileName); }

        }

        [EntityProperty(EntityPropertyType.NA)]
        public string FileInfo
        {
            get { return FilePath + "|" + FileId + "|" + FileSubject + "|" + FileAction; }

        }

        public string FileAction { get; set; }
        [EntityProperty(EntityPropertyType.Identity)]
        public int FileId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        public string FileName { get; set; }
        public int AccountId { get; set; }
        public string FileSubject { get; set; }
        public int Pid { get; set; }
        public string MediaType { get; set; }
        public string FilePath { get; set; }
        public string RefType { get; set; }
        public int RefId { get; set; }

        [EntityProperty(EntityPropertyType.View)]
        public DateTime Creation { get; set; }

        public int UserId { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string UserName { get; set; }
    }


    public class MediaSystem : IEntityItem
    {
       
        public string FilePrefix
        {
            get { return RefType + "_" + RefId.ToString() + "_" + Pid.ToString() + "_"; }
        }
        public string GetFileName(string filename, string fileExt)
        {
            return FilePrefix + filename + fileExt;
        }
        public string RootFolder { get; set; }
        public string Folder { get; set; }
        public string RefType { get; set; }
        public int RefId { get; set; }
        private int Pid { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string UserName { get; set; }
        public bool ReadOnly { get; set; }
    }

    public enum MediaRefTypes : byte
    {
        Task = (byte)'t',
        Project = (byte)'p',
        Leads = (byte)'l'
    }

}
