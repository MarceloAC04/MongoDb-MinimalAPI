using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace minimalAPIMongo.Properties.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateTime? Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        //referência aos produtos do pedido

        //referência para que eu consiga cadastrar um pedido com os produtos
        [BsonElement("productId")]
        [JsonIgnore]
        public List<string>? ProductId { get; set; }

        //referência para listagem de produtos
        [BsonElement("product")]
        public List<Product>? Products { get; set; }

        //referência ao cliente

        [BsonElement("clientId")]
        [JsonIgnore]
        public string? ClientId { get; set; }

        [BsonElement("client")]
        public Client? Client { get; set; }
    }
}
