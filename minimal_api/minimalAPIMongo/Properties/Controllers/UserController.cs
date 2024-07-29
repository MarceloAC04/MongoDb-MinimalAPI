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
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User newUser)
        {
            try
            {
                await _user.InsertOneAsync(newUser);

                return StatusCode(201, newUser);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var users = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(users);
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
                var user = _user.FindOneAndDeleteAsync(p => p.Id == id);

                if (user == null)
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
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                var user = await _user.Find(p => p.Id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(User updatedUser)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq(x => x.Id, updatedUser.Id);

                await _user.ReplaceOneAsync(filter, updatedUser);

                return Ok();


            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

    }
}
