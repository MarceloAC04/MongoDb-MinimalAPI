using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Properties.Domains;
using minimalAPIMongo.Properties.Services;
using minimalAPIMongo.ViewModels;
using MongoDB.Driver;

namespace minimalAPIMongo.Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderViewModel orderView)
        {
            try
            {
                Order order = new Order();

                order.Id = orderView.Id;
                order.Date  = orderView.Date;
                order.Status = orderView.Status;
                order.ProductId = orderView.ProductId;
                order.ClientId = orderView.ClientId;

                var client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound();
                }
                
                order.Client = client;
                
                await _order.InsertOneAsync(order);

                return StatusCode(201, order);
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
