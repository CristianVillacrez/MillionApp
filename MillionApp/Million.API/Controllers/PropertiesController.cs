using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Million.Application.DTOs;
using Million.Application.Filters;
using Million.Application.Interfaces;
using MongoDB.Bson;
namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyRepository _repo;
        private readonly IMapper _mapper;
        public PropertiesController(IPropertyRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? name, [FromQuery] string? address,
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var filter = new PropertyFilter
            {
                Name = name,
                Address = address,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var (items, total) = await _repo.GetPropertiesAsync(filter, page, pageSize);

            // map BsonDocument -> DTO using AutoMapper (registered mapping)
            var list = items.Cast<BsonDocument>().Select(b => _mapper.Map<PropertyListItemDto>(b)).ToList();

            return Ok(new { items = list, totalCount = total, page, pageSize });
        }
    }
}
