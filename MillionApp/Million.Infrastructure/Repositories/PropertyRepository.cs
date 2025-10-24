using Million.Application.Filters;
using Million.Application.Interfaces;
using Million.Domain.Entities;
using Million.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Million.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly MongoDbContext _context;

        public PropertyRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<object> Items, long Total)> GetPropertiesAsync(PropertyFilter filter, int page, int pageSize)
        {
            var matchFilters = new List<FilterDefinition<Property>>();

            var builder = Builders<Property>.Filter;

            if (!string.IsNullOrWhiteSpace(filter?.Name))
                matchFilters.Add(builder.Regex(x => x.Name, new BsonRegularExpression(filter.Name, "i")));

            if (!string.IsNullOrWhiteSpace(filter?.Address))
                matchFilters.Add(builder.Regex(x => x.Address, new BsonRegularExpression(filter.Address, "i")));

            if (filter?.MinPrice != null)
                matchFilters.Add(builder.Gte(x => x.Price, filter.MinPrice.Value));

            if (filter?.MaxPrice != null)
                matchFilters.Add(builder.Lte(x => x.Price, filter.MaxPrice.Value));


            var finalMatch = matchFilters.Count > 0 ? builder.And(matchFilters) : builder.Empty;


            var pipeline = new List<BsonDocument>();

            var matchDoc = finalMatch.Render(
                new RenderArgs<Property>(_context.Properties.DocumentSerializer, _context.Properties.Settings.SerializerRegistry))
                .ToBsonDocument();

            pipeline.Add(new BsonDocument("$match", matchDoc));

            // Buscar Owner
            pipeline.Add(new BsonDocument("$lookup",
                new BsonDocument {
                { "from", "owners" },
                { "localField", "IdOwner" },   // en properties
                { "foreignField", "_id" },     // en owners
                { "as", "owner" }
            }));

            // Unwind para tomar el primer owner 
            pipeline.Add(new BsonDocument("$unwind", new BsonDocument {
                { "path", "$owner" },
                { "preserveNullAndEmptyArrays", true }
            }));


            pipeline.Add(new BsonDocument("$lookup",
                new BsonDocument {
                    { "from", "propertyImages" },
                    { "localField", "_id" },//en prpieties
                    { "foreignField", "IdProperty" },//en images
                    { "as", "images" }
             }));

            // Se toma solo una imagen 
            pipeline.Add(new BsonDocument("$unwind", new BsonDocument {
                    { "path", "$images" },
                    { "preserveNullAndEmptyArrays", true }
            }));


            var project = new BsonDocument("$project", new BsonDocument {
            { "_id", 0 },
            { "IdProperty", "$_id" },
            { "Name", 1 },
            { "Address", 1 },
            { "Price", 1 },
            { "OwnerName", new BsonDocument("$ifNull", new BsonArray { "$owner.Name", "" }) },
            { "Image", new BsonDocument("$ifNull", new BsonArray { "$images.File", "" }) },
            { "IdOwner", 1 }
             });

            var skip = (page - 1) * pageSize;
            pipeline.Add(project);
            pipeline.Add(new BsonDocument("$skip", skip));
            pipeline.Add(new BsonDocument("$limit", pageSize));

            var agg = _context.Properties.Database.GetCollection<BsonDocument>("properties")
                        .Aggregate<BsonDocument>(pipeline);

            var items = await agg.ToListAsync();

            // se obtiene el recuento total por separado 
            var total = await _context.Properties.CountDocumentsAsync(finalMatch);

            return (items, total);
        }

        public async Task SeedSampleDataAsync()
        {
            // comprobar si hay datos
            var exists = await _context.Properties.Find(FilterDefinition<Property>.Empty).AnyAsync();
            if (exists) return;

            // Creo ejemplos de propietarios, propiedades e imagenes.

            var owner1 = Guid.NewGuid();
            var owner2 = Guid.NewGuid();

            var owner = new List<Owner>
            {
                new Owner
                {
                    IdOwner = owner1,
                    Name = "Juan Perez",
                    Address = "Calle 1",
                    Photo = "",
                    Birthday = DateTime.Parse("1980-01-01")
                },
                new Owner
                {
                    IdOwner = owner2,
                    Name = "Tatiana Yepez",
                    Address = "Calle 41",
                    Photo = "",
                    Birthday = DateTime.Parse("1990-01-01")
                }
            };

            await _context.Owners.InsertManyAsync(owner);

            var prop1 = Guid.NewGuid();
            var prop2 = Guid.NewGuid();

            var property = new List<Property>
            {
                new Property
                {
                    IdProperty = prop1,
                    Name = "Casa del Lago",
                    Address = "calle 22",
                    Price = 250000M,
                    CodeInternal = "C-001",
                    Year = 2010,
                    IdOwner = owner1
                },
                new Property
                {
                    IdProperty = prop2,
                    Name="Alambra",
                    Address= "calle 3",
                    Price=5600000,
                    CodeInternal="C0.45",
                    Year=2020,
                    IdOwner= owner2
                }
            };

            await _context.Properties.InsertManyAsync(property);

            var img = new List<PropertyImage>
            {
                new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    IdProperty = prop1,
                    File = "https://cdn.millionluxury.com/image-resizing?image=https://azfd-prod.millionluxury.com/mls/419277547_1.jpg",
                    Enabled = true
                },
                    new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    IdProperty = prop2,
                    File = "https://cdn.millionluxury.com/image-resizing?image=https://azfd-prod.millionluxury.com/mls/407228689_1.jpg",
                    Enabled = true
                }
            };

            await _context.PropertyImages.InsertManyAsync(img);

            var trace = new List<PropertyTrace>
            {
                new PropertyTrace
                {
                    IdPropertyTrace = Guid.NewGuid(),
                    DateSale = DateTime.Today,
                    Name = "",
                    Value = 3000000,
                    Tax = 25000,
                    IdProperty = prop1,
                },
                new PropertyTrace
                {
                    IdPropertyTrace = Guid.NewGuid(),
                    DateSale = DateTime.Today,
                    Name = "",
                    Value = 6000000,
                    Tax = 38000,
                    IdProperty = prop2,
                }
            };

            await _context.PropertyTraces.InsertManyAsync(trace);
        }
    }
}
