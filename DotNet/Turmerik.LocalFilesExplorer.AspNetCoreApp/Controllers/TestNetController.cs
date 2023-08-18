using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Turmerik.LocalFilesExplorer.AspNetCoreApp.Controllers
{
    [Serializable]
    public class TestNetData
    {
        public DateTime ReqDate { get; set; }
        public DateTime? RespDate { get; set; }
    }

    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TestNetController : ControllerBase
    {
        [Route("[action]")]
        [HttpGet]
        public IEnumerable<string> GetSomethingElse(TestNetData data)
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/<TestNetController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TestNetController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestNetController>
        [HttpPost]
        public void Post([FromBody] TestNetData data)
        {
        }

        // PUT api/<TestNetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] TestNetData data)
        {
        }

        // DELETE api/<TestNetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
