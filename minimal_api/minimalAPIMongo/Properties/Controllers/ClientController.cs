using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Properties.Domains;
using minimalAPIMongo.Properties.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client> _client;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Create([FromBody] Client newClient)
        {
            try
            {
                await _client.InsertOneAsync(newClient);

                return StatusCode(201, newClient);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var clients = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(clients);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Deletar(string id)
        {
            try
            {
                var client = _client.FindOneAndDeleteAsync(p => p.Id == id);

                if (client == null)
                {
                    return NotFound();
                }

                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<Client>> GetById(string id)
        {
            try
            {
                var client = await _client.Find(p => p.Id == id).FirstOrDefaultAsync();
                if (client == null)
                {
                    return NotFound();
                }

                return Ok(client);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("userId")]
        public async Task<ActionResult<Client>> GetByUserId(string id)
        {
            try
            {
                var client = await _client.Find(u => u.UserId == id).FirstOrDefaultAsync();
                if (client == null)
                {
                    return NotFound();
                }

                return Ok(client);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Client updatedClient)
        {
            try
            {
                var filter = Builders<Client>.Filter.Eq(x => x.Id, updatedClient.Id);

                await _client.ReplaceOneAsync(filter, updatedClient);

                return Ok();


            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
