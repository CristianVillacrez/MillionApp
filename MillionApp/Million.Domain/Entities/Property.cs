using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Million.Domain.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid IdProperty { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public Guid IdOwner { get; set; }

       
    }
}