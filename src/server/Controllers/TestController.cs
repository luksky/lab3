using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Core;
using server.Core.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {

        
        [Authorize]
        [HttpPost]
        public ActionResult Post([FromBody] DataResource dataResource)
        {
            return Ok(dataResource);
        }

        [Authorize(Policies.RequireAdmin)]
        [HttpDelete("{value}")]
        public ActionResult<string> Delete(string input)
        {
            return Ok($"{input} usunięte");
        }
    }
}
