using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("GithubLib")]
    public class GithubLib : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public string Get(string s)
        {
            return "GithubLib";
        }

        [HttpGet]
        [Route("token")]
        public string CallbackGetToken(string code)
        {
            return code;
        }
    }
}
