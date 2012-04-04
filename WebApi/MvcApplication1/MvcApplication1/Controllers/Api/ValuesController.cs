using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers.Api {
    public class ValuesController : ApiController {
        // GET /api/values
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET /api/values/5
        public string Get(int id) {
            return "value";
        }

        // POST /api/values
        public void Post(Person newPerson) {
            string debug = "foo";
        }

        // PUT /api/values/5
        public void Put(int id, string value) {
            string debug = "foo";
        }

        // DELETE /api/values/5
        public void Delete(int id) {
        }
    }
}