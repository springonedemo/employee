using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudFoundry.ViewModels.Values;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        private CloudFoundryServicesOptions CloudFoundryServices { get; set; }
        private CloudFoundryApplicationOptions CloudFoundryApplication { get; set; }

        public ValuesController(
            IOptions<CloudFoundryApplicationOptions> appOptions,
            IOptions<CloudFoundryServicesOptions> servOptions)
        {
            CloudFoundryServices = servOptions.Value;
            CloudFoundryApplication = appOptions.Value;
        }

        [HttpGet]
        public SampleData Get()
        {

            SampleData s = new SampleData();
            s.CompanyName = "Capgemini";
            //s.ID = 1;
            return s;
            //return new string[] { "Hi I am one of the Microservice from another Microservice by using Steelsteo"};
            //return new string[] { "Hi I am value from Another Microservice", "value2" };
        }
        // GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
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

        public IActionResult CloudFoundry()
        {
            return View(new CloudFoundryViewModel(
                CloudFoundryApplication == null ? new CloudFoundryApplicationOptions() : CloudFoundryApplication,
                CloudFoundryServices == null ? new CloudFoundryServicesOptions() : CloudFoundryServices));
        }
    }
}
