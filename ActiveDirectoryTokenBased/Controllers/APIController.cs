using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ActiveDirectoryTokenBased.Controllers
{
    public class APIController : ApiController
    {
        [Authorize(Roles ="user")]
        [Route("IsTokenAuthorized")]
        public IHttpActionResult Get()
        {
            return Ok("True");
        }
    }
}
