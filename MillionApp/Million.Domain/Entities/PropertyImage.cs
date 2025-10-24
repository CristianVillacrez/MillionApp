using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Million.Domain.Entities
{
    public class PropertyImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid IdPropertyImage { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid IdProperty { get; set; }

        public string File { get; set; }
        public bool Enabled { get; set; }
    }
}
