using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Properties.Domains;
using minimalAPIMongo.Properties.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] Order newOrder)
        {
            try
            {
                await _order.InsertOneAsync(newOrder);

                return StatusCode(201, newOrder);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
                return Ok(orders);
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
                var order = _order.FindOneAndDeleteAsync(p => p.Id == id);

                if (order == null)
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
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                var order = await _order.Find(p => p.Id == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("clientId")]
        public async Task<ActionResult<Order>> GetByClientId(string id)
        {
            try
            {
                var order = await _order.Find(p => p.Client!.Id == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Order updatedOrder)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, updatedOrder.Id);

                await _order.ReplaceOneAsync(filter, updatedOrder);

                return Ok();


            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
