using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFFA_API
{
    public interface IAppSettings
    {
        public string SecretGet { get; }
        public string Issuer { get; }
        public string Audition { get;  }
    }
    public class AppSettings
    {
        public static string Secret;
        public static string Issuer { get => "Dasda"; }
        public static string Audition { get => "Dasda"; }
        public static string EmailSenderAddress;
        public static string EmailSenderPassword;
    }
}
