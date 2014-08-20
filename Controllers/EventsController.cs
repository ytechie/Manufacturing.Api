using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Manufacturing.Api.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/events/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/events
        public void Post([FromBody]string value)
        {
        }

        // PUT api/events/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/events/5
        public void Delete(int id)
        {
        }
    }
}
