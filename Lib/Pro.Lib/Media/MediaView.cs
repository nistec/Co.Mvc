using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;
using Nistec.Web.Controls;

namespace Pro.Data.Entities
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

    [EntityMapping("Media_Files", "vw_Media_Files")]
    public class MediaFile : IEntityItem
    {
        public static IList<MediaFile> GetList(int pid, int userId, int ttl)
        {
            string key = DbContextCache.GetKey<MediaFile>(Settings.ProjectName, EntityCacheGroups.Task, 0, userId);
            return DbContextCache.EntityList<DbSystem, MediaFile>(key, ttl, "Pid", pid);
        }
        public static MediaFile Get(int Pid, string FileId)
        {
            if (!string.IsNullOrEmpty(FileId) && Pid > 0)
                return new MediaFile() { Pid = Pid };

            return DbContext.EntityGet<DbSystem, MediaFile>("FileId", FileId);
        }
        public static int Save(int Pid, string FileId, MediaFile entity, UpdateCommandType commandType)
        {
            return DbContext.EntitySave<DbSystem, MediaFile>(entity, commandType, new object[] { "Pid", Pid, "FileId", FileId });
        }
        public static int Delete(int FileId)
        {
            return DbContext.EntityDelete<DbSystem, MediaFile>("FileId", FileId);
        }

        internal const string MappingName = "Task_Files";


        [EntityProperty(EntityPropertyType.Identity)]
        public int FileId { get; set; }
        public string FileSubject { get; set; }
        public int Pid { get; set; }
        public string Ptype { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ReferralType { get; set; }
        public string ReferralKey { get; set; }

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
            get { return ReferralType + "_" + ReferralKey + "_"; }
        }
        public string GetFileName(string filename, string fileExt)
        {
            return ReferralType + "_" + ReferralKey + "_" + filename + fileExt;
        }
        public string RootFolder { get; set; }
        public string AccountFolder { get; set; }
        public string ReferralType { get; set; }
        public string ReferralKey { get; set; }
        public int UserId { get; set; }
        [EntityProperty(EntityPropertyType.View)]
        public string UserName { get; set; }
    }

 
}
