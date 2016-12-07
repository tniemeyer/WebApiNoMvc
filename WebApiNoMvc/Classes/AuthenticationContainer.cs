using System;
using WebApiNoMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiNoMvc.Classes
{
    public class AuthenticationContainer
    {
        public static Dictionary<string, string> credentials = new Dictionary<string, string>()
        { 
            { "Todd", "toddspwd" },
            { "Finley", "finspwd" }
        };

        // Map of user to token
        public static Dictionary<string, Token> activeTokens = new Dictionary<string, Token>();
    }
}