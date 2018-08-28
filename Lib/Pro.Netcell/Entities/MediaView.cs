using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nistec.Data.Entities;
using Pro.Data;
using Nistec.Data;

namespace Pro.Data.Entities
{

    [Entity(EntityName = "MediaView", MappingName = "Party_Media", ConnectionKey = "cnn_pro", EntityKey = new string[] { "MediaId,AccountId" })]
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
            return DbNatam.Instance.ExecuteCommand("delete from Crm_Media where MediaId=@MediaId", "MediaId", id);
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

        public static IEnumerable<MediaView> View(int buildingId, int propertyId, string propertyType)
        {
            if (propertyType == "u")
                return ViewByProperty(propertyId);
            if (propertyType == "b")
                return ViewByBuilding(buildingId);
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
        #endregion

    }

    public class MediaView : IEntityItem
    {

        #region properties

        [EntityProperty(EntityPropertyType.Identity)]
        public int MediaId { get; set; }
        [EntityProperty]
        public int AccountId { get; set; }
        [EntityProperty]
        public string MediaType { get; set; }

        [EntityProperty]
        [Validator("נתיב מדיה",true)]
        public string MediaPath { get; set; }

        #endregion
    }

    public class MemberUploadView : IEntityItem
    {

        [EntityProperty(EntityPropertyType.Key)]
        public string MemberId { get; set; }
        [EntityProperty(EntityPropertyType.Key)]
        //public int AccountId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public int PlaceOfBirth { get; set; }
        //public int BirthDateYear { get; set; }
        public int ChargeType { get; set; }
        public int Branch { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public int Region { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Note { get; set; }
        public string Categories { get; set; }
        public DateTime JoiningDate { get; set; }

    }
 
}
