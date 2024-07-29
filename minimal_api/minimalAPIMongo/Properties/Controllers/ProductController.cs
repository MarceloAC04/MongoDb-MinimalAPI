using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Properties.Domains;
using minimalAPIMongo.Properties.Services;
using MongoDB.Driver;
using System.Reflection;

namespace minimalAPIMongo.Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Armazena os dados de acesso da collection
        /// </summary>
        private readonly IMongoCollection<Product> _product;


        /// <summary>
        /// Construtor que recebe como dependência o obj da classe MongoDbService
        /// </summary>
        /// <param name="mongoDbService"></param>
        public ProductController(MongoDbService mongoDbService)
        {
            //obtem a collection "product"
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product newProduct)
        {
            try
            {
                await _product.InsertOneAsync(newProduct);

                return StatusCode(201, newProduct);
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
                var product = _product.FindOneAndDeleteAsync(p => p.Id == id);

                if (product == null)
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
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                var product = await _product.Find(p => p.Id == id).FirstOrDefaultAsync();
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Product updatedProduct)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(x => x.Id, updatedProduct.Id);

                await _product.ReplaceOneAsync(filter, updatedProduct);

                return Ok();


            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
