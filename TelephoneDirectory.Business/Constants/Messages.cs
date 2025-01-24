using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephoneDirectory.Business.Constants
{
    public static class Messages
    {
        public static string PersonAdded = "Kişi başarılı bir şekilde eklendi";
        public static string PersonUpdated = "Kişi başarılı bir şekilde güncellendi";
        public static string PersonDeleted = "Kişi başarılı bir şekilde silindi";
        public static string PersonsListed = "Kişiler listelendi";
        public static string AuthorizationDenied = "You do not have authority";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Şifre hatalı";
        public static string SuccessfulLogin = "Sisteme giriş başarılı";
        public static string UserAlreadyExists = "Bu kullanıcı zaten mevcut";
        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi";
        public static string AccessTokenCreated = "Access token başarıyla oluşturuldu";
    }
}
