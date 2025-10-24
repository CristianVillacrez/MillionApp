using Microsoft.Extensions.Options;
using Million.Domain.Entities;
using Million.Infrastructure.Settings;
using MongoDB.Driver;

namespace Million.Infrastructure.Data
{
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; }
        public MongoDbContext(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Property> Properties => Database.GetCollection<Property>("properties");
        public IMongoCollection<Owner> Owners => Database.GetCollection<Owner>("owners");
        public IMongoCollection<PropertyImage> PropertyImages => Database.GetCollection<PropertyImage>("propertyImages");
        public IMongoCollection<PropertyTrace> PropertyTraces => Database.GetCollection<PropertyTrace>("propertyTraces");
    }
}
