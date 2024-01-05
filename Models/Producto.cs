using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CRUD_NoSQL.Models
{
    public class Producto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
    }
}