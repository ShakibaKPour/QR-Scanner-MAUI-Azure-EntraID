using Newtonsoft.Json;
using RepRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepRepair.Services.Configuration
{
    public class ConfigurationService
    {
        private readonly HttpClient _httpClient;

        //private AppConfig AppConfig;
        
        public AppConfig AppConfig
        {
            get;
            private set;
        }

        private readonly string _configUrl =
            "https://reprepair.azurewebsites.net/api/GetAppConfig?code=5-fq97Kbxc--gdLH2Q2meNnV0j3BvzVbO3c_QTnvycY6AzFufKx-jw==";

        public ConfigurationService()
        {
                _httpClient = new HttpClient();
        }

        public async Task<AppConfig> GetAppConfiguration()
        {
            if (AppConfig != null) return AppConfig;

            try
            {
                var response = await _httpClient.GetStringAsync(_configUrl);
                Console.WriteLine("Response not reaching further"+ response);
                var _fetchedAppConfig = JsonConvert.DeserializeObject<AppConfig>(response);
                AppConfig = _fetchedAppConfig;
                return AppConfig;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not fetch app configuration", ex);
            }

        }
        public AppConfig GetCurrentConfiguration() 
        {
            if (AppConfig == null)
                throw new InvalidOperationException("Could not fetch app configuration");

            return AppConfig;
        }

    }
}
