using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepRepair.Models
{
    public class AppConfiguration
    {
        public bool UseAuthentication { get; set; }
        public string BaseFunctionUrlGet { get; set; }
        public string BaseFunctionUrlPost { get; set; }
        public string BaseFunctionUrlGetLanguages { get; set; }
        public string BaseFunctionUrlGetReportTypes { get; set; }
        public string BaseFunctionUrlGetDefectList { get; set; }

        // You can add more configuration properties as needed.

        // Example constructor to set default values
        // These could also be set from an external configuration source
        public AppConfiguration()
        {
            UseAuthentication = true; // Default to using authentication, can be overridden by external configuration
            BaseFunctionUrlGet = "https://reprepair.azurewebsites.net/api/getobjectinfo/qrcode/";
            BaseFunctionUrlPost = "https://reprepair.azurewebsites.net/api/InsertToReportAndLinkTables?";
            BaseFunctionUrlGetLanguages = "https://reprepair.azurewebsites.net/api/GetAvailableLanguages?";
            BaseFunctionUrlGetReportTypes = "https://reprepair.azurewebsites.net/api/GetReportTypes?";
            BaseFunctionUrlGetDefectList = "https://reprepair.azurewebsites.net/api/GetDefectList?";
        }
    }

}
