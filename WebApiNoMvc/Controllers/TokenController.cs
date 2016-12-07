using System;
using WebApiNoMvc.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiNoMvc.Controllers
{
    public class TokenController : ApiController
    {
        Dictionary<string, string> credentials = new Dictionary<string, string>
        {
            { "Todd", "toddspwd" },
            { "Finley", "finspwd" }
        };
        
        // GET: api/Token/5
        // Essentially a login method -- give creds / get token
        public IHttpActionResult Get(string username, string password)
        {
            string passwordFromStore;
            if (credentials.TryGetValue(username, out passwordFromStore) && 
                passwordFromStore == password)
            {
                var newToken = new Models.Token() { Value = username + "TOKEN" };
                AuthenticationContainer.activeTokens[username] = newToken;
                return Ok(newToken);
            }
            return Unauthorized();
        }
    }
}
