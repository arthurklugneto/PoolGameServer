using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PoolGameServer.Entities;
using PoolGameServer.Persistence;
using PoolGameServer.Repositories;

namespace PoolGameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;

        public ValuesController(IUserRepository userRepository, IUnitOfWork uow)
        {
            _userRepository = userRepository;
            _uow = uow;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            
            var novoUsuario = new User(); 
            novoUsuario.Balance = new UserBalance(){
                CoinsPool = 300,
                StarsPool = 5
            };
            novoUsuario.Name = "Arthur Klug Neto";
            novoUsuario.Password = "A6D87D7DABHDA";
            novoUsuario.Email = "arthurklugneto@gmail.com";

            _userRepository.Add(novoUsuario);
            await _uow.Commit();
            

            return "OK";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
