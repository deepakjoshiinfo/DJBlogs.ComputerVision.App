using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Utility
{
    public class AppConstant
    {
        public static string CloudBlobContainer = Environment.GetEnvironmentVariable("CloudBlobContainer");
        public static string CloudBlobAccountName = Environment.GetEnvironmentVariable("CloudBlobAccountName"); 
        public static string CloudBlobKey = Environment.GetEnvironmentVariable("CloudBlobKey");
        public static string CloudBlobContainerURL = string.Format("https://{0}.blob.core.windows.net/{1}", CloudBlobAccountName, CloudBlobContainer);

        public static string Key = Environment.GetEnvironmentVariable("Key");
        public static string Endpoint = Environment.GetEnvironmentVariable("Endpoint");
    }
}
