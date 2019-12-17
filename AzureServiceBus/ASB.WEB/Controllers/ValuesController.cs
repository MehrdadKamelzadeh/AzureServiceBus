using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASB.Services;

namespace ASB.WEB.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly ISaveSomethingService _saveSomethingService;

        public ValuesController(ISaveSomethingService saveSomethingService )
        {
            _saveSomethingService = saveSomethingService;
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            _saveSomethingService.Save();
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public void RegisterHandlers()
        {
           _saveSomethingService.RegisterHandler();
        }
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
