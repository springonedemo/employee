using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudFoundry.ViewModels.Values;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IOptionsSnapshot<ConfigServerData> IConfigServerData { get; set; }

        private ConfigServerClientSettingsOptions ConfigServerClientSettingsOptions { get; set; }
        private IConfigurationRoot Config { get; set; }


        private CloudFoundryServicesOptions CloudFoundryServices { get; set; }
        private CloudFoundryApplicationOptions CloudFoundryApplication { get; set; }

        public ValuesController(IConfigurationRoot config,
            IOptionsSnapshot<ConfigServerData> configServerData,
            IOptions<CloudFoundryApplicationOptions> appOptions,
            IOptions<CloudFoundryServicesOptions> servOptions,
            IOptions<ConfigServerClientSettingsOptions> confgServerSettings)
        {
            
            // The ASP.NET DI mechanism injects the data retrieved from the
            // Spring Cloud Config Server as an IOptionsSnapshot<ConfigServerData>
            // since we added "services.Configure<ConfigServerData>(Configuration);"
            // in the StartUp class
            if (configServerData != null)
                IConfigServerData = configServerData;

            // The ASP.NET DI mechanism injects these as well, see
            // public void ConfigureServices(IServiceCollection services) in Startup class
            if (servOptions != null)
                CloudFoundryServices = servOptions.Value;
            if (appOptions != null)
                CloudFoundryApplication = appOptions.Value;

            // Inject the settings used in communicating with the Spring Cloud Config Server
            if (confgServerSettings != null)
                ConfigServerClientSettingsOptions = confgServerSettings.Value;

            Config = config;
        }

        [HttpGet]
        public SampleData Get()
        {

            SampleData s = new SampleData();
            s.CompanyName = "Capgemini";
            //s.ID = 1;
            return s;
        }
        [HttpGet("{id}")]
        public SampleData Get(int id)
        {
            SampleData s = new SampleData();

            switch (id)
            {
                case 1:
                    s.CompanyName = "Capgemini";

                    s.EmployeeName = "Ramu";
                    s.Location = "Mumbai";
                    s.EmployeeID = "4653";
                    s.EmployeeRole = "Developer";
                    break;
                case 2:
                    s.CompanyName = "Sogeti";

                    s.EmployeeName = "Charan";
                    s.Location = "Bangalore";
                    s.EmployeeID = "9823";
                    s.EmployeeRole = "Tester";
                    break;
                case 3:
                    s.CompanyName = "Capgemini Consulting";

                    s.EmployeeName = "Karan";
                    s.Location = "Delhi";
                    s.EmployeeID = "4577";
                    s.EmployeeRole = "HR";
                    break;
                //case 4:
                //    s.CompanyName = "Capgemini";

                //    s.EmployeeName = "Ramu";
                //    s.Location = "Location";
                //    s.EmployeeID = "12345";
                //    s.EmployeeRole = "Developer";
                //    break;
            }

            return s;
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        public IActionResult ConfigServerSettings()
        {
            if (ConfigServerClientSettingsOptions != null)
            {
                ViewData["AccessTokenUri"] = ConfigServerClientSettingsOptions.AccessTokenUri;
                ViewData["ClientId"] = ConfigServerClientSettingsOptions.ClientId;
                ViewData["ClientSecret"] = ConfigServerClientSettingsOptions.ClientSecret;
                ViewData["Enabled"] = ConfigServerClientSettingsOptions.Enabled;
                ViewData["Environment"] = ConfigServerClientSettingsOptions.Environment;
                ViewData["FailFast"] = ConfigServerClientSettingsOptions.FailFast;
                ViewData["Label"] = ConfigServerClientSettingsOptions.Label;
                ViewData["Name"] = ConfigServerClientSettingsOptions.Name;
                ViewData["Password"] = ConfigServerClientSettingsOptions.Password;
                ViewData["Uri"] = ConfigServerClientSettingsOptions.Uri;
                ViewData["Username"] = ConfigServerClientSettingsOptions.Username;
                ViewData["ValidateCertificates"] = ConfigServerClientSettingsOptions.ValidateCertificates;
            }
            else
            {
                ViewData["AccessTokenUri"] = "Not Available";
                ViewData["ClientId"] = "Not Available";
                ViewData["ClientSecret"] = "Not Available";
                ViewData["Enabled"] = "Not Available";
                ViewData["Environment"] = "Not Available";
                ViewData["FailFast"] = "Not Available";
                ViewData["Label"] = "Not Available";
                ViewData["Name"] = "Not Available";
                ViewData["Password"] = "Not Available";
                ViewData["Uri"] = "Not Available";
                ViewData["Username"] = "Not Available";
                ViewData["ValidateCertificates"] = "Not Available";
            }
            return View();
        }

        public IActionResult CloudFoundry()
        {
            return View(new CloudFoundryViewModel(
                CloudFoundryApplication == null ? new CloudFoundryApplicationOptions() : CloudFoundryApplication,
                CloudFoundryServices == null ? new CloudFoundryServicesOptions() : CloudFoundryServices));
        }
    }
}
