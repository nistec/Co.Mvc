using Nistec;
using Nistec.Data;
using Nistec.Data.Entities;
using Nistec.Logging;
using Nistec.Web.Security;
using ProNetcell.Data;
using ProNetcell.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Pro.Netcell.Api
{
    public class ApiService //: IContactService
    {
        string ClientIP = null;

        public ApiService(string clientId)
        {
            ClientIP = clientId;
        }

        public string ReadStatus(int status)
        {
            if (status > 0)
                return "Member updated";
            else if (status == 0)
                return "Member allready exists";
            else
                return "Member has not been updated";
        }

        public ApiContactStatus MemberAdd(MemberMessageRequest m)
        {
            try
            {
                if (m == null)
                {
                    throw new ArgumentException("Message parsing error");
                }
                m.ValidateMessage(ClientIP);

                Netlog.InfoFormat("-MemberAdd- AccountId={0}, CellNumber={1}, Email={2},Category={3}", m.Auth.AccountId, m.Body.CellPhone, m.Body.Email, m.Body.Category);

                //DataSourceTypes DataSourceType= DataSourceTypes.ApiSync;
               string DataSource = "api-" + ClientIP;
               int RecordId=0;

               var args = new object[]{
                "RecordId", RecordId
                ,"AccountId", m.Auth.AccountId
                ,"MemberId", m.Body.MemberId
                ,"LastName", m.Body.LastName
                ,"FirstName", m.Body.FirstName
                ,"Address", m.Body.Address
                ,"City", m.Body.City
                ,"CellPhone",m.Body.CellPhone
                ,"Phone", m.Body.Phone
                ,"Email", m.Body.Email
                ,"Gender",MemberContext.ReadGender(m.Body.Gender)
                ,"Birthday", m.Body.Birthday
                ,"Note", m.Body.Note
                //,"JoiningDate", DateTime.Now
                ,"Branch", m.Body.Branch
                ,"ZipCode", m.Body.ZipCode
                ,"ContactRule", 0
                ,"Category", m.Body.Category
                ,"ExField1", m.Body.ExField1
                ,"ExField2", m.Body.ExField2
                ,"ExField3", m.Body.ExField3
                ,"ExEnum1", m.Body.ExEnum1
                ,"ExEnum2", m.Body.ExEnum2
                ,"ExEnum3", m.Body.ExEnum3
                ,"ExId", m.Body.ExId
                ,"UpdateType",(int)MemberContext.ReadMemberUpdateType(m.Body.UpdateType,MemberUpdateType.Sync)
                ,"EnableNews", m.Body.EnableNews
                ,"DataSource", "api-" + ClientIP
                ,"DataSourceType",(int) DataSourceTypes.ApiSync// tinyint=0-- CoSystem = 0,Register = 1,FileSync = 2,ApiSync = 3
                ,"MemberType", m.Body.MemberType
                ,"CompanyName", m.Body.CompanyName
                ,"ExRef1", m.Body.ExRef1
                ,"ExRef2", m.Body.ExRef2
                ,"ExRef3", m.Body.ExRef3
                };

               

                var parameters=DataParameter.GetSqlList(args);
                parameters[0].Direction= System.Data.ParameterDirection.InputOutput;
                //DbCo.Instance.ExecuteNonQuery("sp_Member_Sync_v1", parameters.ToArray(), System.Data.CommandType.StoredProcedure);

                using (var db = DbContext.Create<DbPro>())
                {
                    var res = db.ExecuteCommandOutput("sp_Member_Sync_v1", parameters.ToArray(), System.Data.CommandType.StoredProcedure);
                    RecordId = res.GetValue<int>("RecordId");
                }

                //RecordId = Types.ToInt(parameters[0].Value);


                //ApiAck ack = ContactContext.ContactAdd(m.Body.GetContact(m.Auth.AccountId), m.Body.GroupName, updateType);
                //ApiAck ack = ContactContext.ContactAddLight(0, ua.AccountId, message.CellNumber, message.FirstName, message.LastName, message.BirthDate, message.Email, message.City, message.Details, null, message.Address, message.Sex, ContactSignType.Api, "Rest.ContactService", message.Company, enableNews, message.GroupName);
                //int ContactId = ack.Id;

                return ApiContactStatus.Get(RecordId.ToString(), RecordId > 0 ? 1 : 0, ReadStatus(RecordId));//RecordId > 0 ? "Member updated" : "Member has not been updated");

            }
            catch (ApiException mex)
            {
                Netlog.Exception("-MemberAdd-AppException ", mex);
                return ApiContactStatus.Get(mex, -1);
            }
            catch (ArgumentException aex)
            {
                Netlog.Exception("-MemberAdd-ArgumentException ", aex);
                return ApiContactStatus.Get(aex, -1);
            }
            catch (SecurityException sex)
            {
                Netlog.Exception("-MemberAdd-SecurityException ", sex);
                return ApiContactStatus.Get(sex, -1);
            }
            catch (Exception ex)
            {
                Netlog.Exception("-MemberAdd-Exception", ex);
                return ApiContactStatus.Get(null, -1, "Exception: Internal server error");
            }
        }

    }
}