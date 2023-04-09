using BlazorCRUDApp.Server.Models;
using BlazorCRUDApp.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCRUDApp.Server.Controllers
{
    [Route("webui/api/[controller]")]
    [ApiController]
    public class ScoringController : ControllerBase
    {
        //private readonly IPersonService _personService;
        //public ScoringController(IPersonService personService)
        //{
        //    _personService = personService;
        //}

        [HttpGet]
        public async Task<List<Scoring>> Get()
        {
            //return await _personService.GetAllPersons();
            List<Scoring> p = new List<Scoring>();
            p.Add(new Scoring { Id = 1, Email = "email.cccc", FirstName = "f", LastName = "l", MobileNo = "123" });
            return p;
        }

        [HttpGet("{id}")]
        public async Task<Scoring> Get(int id)
        {
            //return await _personService.GetPerson(id);

            return new Scoring();
        }

        [HttpPost]
        public async Task<Scoring> AddPerson([FromBody] Scoring person)
        {
            return new Scoring();
            //return await _personService.AddPerson(person);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeletePerson(int id)
        {
            //await _personService.DeletePerson(id); return true;
            return true;
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdatePerson(int id, [FromBody] Scoring Object)
        {
            //await _personService.UpdatePerson(id, Object); return true;
            return true;
        }
    }
}
