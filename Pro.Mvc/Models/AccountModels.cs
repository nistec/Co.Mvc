using Nistec.Web.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace Pro.Mvc.Models
{
    public class ProSettings
    {
        public const int AppId = 2;

    }
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfileModel> UserProfiles { get; set; }
    }

    //[Table("UserProfile")]
    public class UserProfileModel
    {
        [Key]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        //[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "דאר אלקטרוני")]
        public string UserName { get; set; }

        //[Required]
        //[Display(Name = "דאר אלקטרוני")]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [Display(Name = "זכור אותי?")]
        public bool RememberMe { get; set; }
        [Display(Name = "הודעה")]
        public string Message { get; set; }
    }

    public class RegisterModel : ResetPasswordModel
    {
        [Required]
        [Display(Name = "שם משתמש")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "שם מלא")]
        public string DisplayName { get; set; }
        [Required]
        [Display(Name = "טלפון נייד")]
        public string Phone { get; set; }

        public string Folder { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        [Display(Name = "דאר אלקטרוני")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "נדרש 6 תוים לפחות לסיסמא.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמא")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "אישור סיסמא")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "הסיסמה ואישור הסימסה אינם תואמים.")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public int AccountId { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        [Display(Name = "דאר אלקטרוני")]
        [EmailAddress(ErrorMessage = "דאר אלקטרוני אינו תקין")]
        public string Email { get; set; }
    }

    //public class ForgotPasswordViewModel
    //{
    //    [Required]
    //    [Display(Name = "User name")]
    //    public string UserName { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm password")]
    //    //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }
    //}

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class FinalModel
    {
        public FinalModel(string type, int code)
        {
            bool isok=false;
            Message = StatusDesc.GetMembershipStatus(type, code, ref isok);
            IsOk = isok;
            if (IsOk)
            {
                Icon = "ok-alert.png";
                Alt = "Ok";
            }
            else
            {
                Icon = "warning.png";
                Alt = "warning";
            }

        }

        public bool IsOk { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Icon { get; set; }
        public string Alt { get; set; }
        public string Link { get; set; }
     }


    public class StatusDesc
    {
        public static string GetMembershipStatus(string type, int code, ref bool IsOk)
        {
            if (type == null)
                type = "";
            type = type.ToLower();
            int Code = code;
            IsOk = false;
            string Message=null;

            if (type == "membershipstatus")
            {
                switch((MembershipStatus)code)
                {
                    case MembershipStatus.Success:
                        IsOk = true;
                        Message = "התהליך הושלם בהצלחה";
                        break;
                    case MembershipStatus.Error:
                        Message = "אירעה שגיאה, נא לנסות שוב, אחרת נא לפנות לתמיכה";
                        break;
                    case MembershipStatus.CouldNotResetPassword:
                         Message = "לא ניתן לאתחל את סיסמתך במערכת, נא לנסות שוב, אחרת נא לפנות לתמיכה";
                        break;
                    case MembershipStatus.DuplicateUserNameOrEmail:
                        Message = "שם משתמש או כתובת דואל כבר קיימים במערכת, נא לבחור שם משתמש או כתובת דואל אחרים";
                        break;
                    case MembershipStatus.InvalidAccountPath:
                        Message = "נתונים שגויים, לא הוגדר נתיב לחשבון הנוכחי, נא לפנות לתמיכה";
                        break;
                    case MembershipStatus.InvalidEmailFormat:
                        Message = "כתובת הדואל אינה תקינה";
                        break;
                    case MembershipStatus.InvalidPasswordFormat:
                        Message = "הסיסמה שהוקלדה אינה תואמת את כללי הסיסמאות במערכת, נא לנסות שוב עם סיסמה שונה";
                        break;
                    case MembershipStatus.UserNameOrEmailNotExists:
                        Message = "שם משתמש או כתובת דואל שציינת אינם מוכרים במערכת, נא לפנות לתמיכה";
                        break;
                    case MembershipStatus.UserRejected:
                        Message = "בקשתך נדחתה, נא לפנות לתמיכה";
                        break;
                    case MembershipStatus.MembershipNotExists:
                        Message = "לא נמצאו הגדרות הזדהות במערכת, נא לפנות לתמיכה";
                        break;
                    case MembershipStatus.UserIsBlocked:
                        Message = "אינך מורשה לשימוש במערכת, נא לפנות למנהל המערכת";
                        break;
                    case MembershipStatus.ResetTokenSent:
                        IsOk = true;
                        Message = "בשלב זה נשלח לכתובת הדואל שלך הודעה לאתחול סיסמה, נא לפעול בהתאם להוראות שנשלחו לתיבת הדאר שלך";
                        break;
                    case MembershipStatus.InvalidUser:
                        Message = "פרטי המשתמש שצויינו אינם מוכרים במערכת, נא לפנות לתמיכה";
                        break;
                    case MembershipStatus.InvalidTokenFormt:
                        Message = "קוד הפנייה לאתחול סיסמה אינו תקין, נא לפנות לתמיכה";
                        break;
                    case MembershipStatus.UserPasswordWasReset:
                        IsOk = true;
                        Message = "סיסמתך אותחלה בהצלחה, כעת ניתן להיכנס למערכת עם פרטי ההזדהות החדשים";
                        break;
                    case MembershipStatus.TokenVerificationExpired:
                        IsOk = true;
                        Message = "פג תוקפו של קוד הפנייה לאתחול סיסמה";
                        break;
               }
            }
            else if (type == "authstate")
            {
                 switch((AuthState)code)
                {
                     case AuthState.Succeeded:
                          IsOk = true;
                        Message = "התהליך הושלם בהצלחה";
                        break;
                     case AuthState.EvaluationExpired:
                        Message = "זמן הנסיון במערכת הסתיים, נא לפנות לתמיכה";
                        break;
                     case AuthState.Failed:
                        Message = "התהליך נכשל";
                        break;
                     case AuthState.IpNotAlowed:
                        Message = "אין הרשאה לפעולה מהכתובת הנוכחית";
                        break;
                     case AuthState.NonConfirmed:
                        Message = "אין אישור לפעולה המבוקשת";
                        break;
                     case AuthState.UnAuthorized:
                        Message = "פרטי ההזדהות אינם מוכרים במערכת";
                        break;
                     case AuthState.UserNotRemoved:
                        Message = "המשתמש לא הוסר";
                        break;
                     case AuthState.UserNotUpdated:
                        Message = "פרטי המשתמש לא עודכנו במערכת";
                        break;
                     case AuthState.UserRemoved:
                        Message = "המשתמש הוסר מהמערכת";
                        break;
                     case AuthState.UserUpdated:
                        Message = "פרטי המשתמש עודכנו";
                        break;
                }
            }


            //if (IsOk)
            //{
            //    Icon = "ok-alert.png";
            //    Alt = "Ok";
            //}
            //else
            //{
            //    Icon = "warning.png";
            //    Alt = "warning";
            //}

            return Message;
        }
    }
}
