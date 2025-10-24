using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Million.API.Controllers;
using Million.Application.DTOs;
using Million.Application.Interfaces;
using Million.Application.Filters;
using Moq;
using MongoDB.Bson;
using NUnit.Framework;


namespace Million.Tests
{
    public class PropertiesControllerTests
    {
        private Mock<IPropertyRepository> _repoMock;
        private IMapper _mapper;
        private PropertiesController _controller;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IPropertyRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BsonDocument, PropertyListItemDto>()
                    .ForMember(d => d.IdProperty, o => o.MapFrom(s => s.GetValue("IdProperty").ToString()))
                    .ForMember(d => d.PropertyName, o => o.MapFrom(s => s.GetValue("Name").AsString))
                    .ForMember(d => d.PropertyAddress, o => o.MapFrom(s => s.GetValue("Address").AsString))
                    .ForMember(d => d.Price, o => o.MapFrom(s => s.GetValue("Price").ToDecimal()))
                    .ForMember(d => d.OwnerName, o => o.MapFrom(s => s.Contains("OwnerName") ? s.GetValue("OwnerName").AsString : string.Empty))
                    .ForMember(d => d.Image, o => o.MapFrom(s => s.Contains("Image") ? s.GetValue("Image").AsString : string.Empty))
                    .ForMember(d => d.IdOwner, o => o.MapFrom(s => s.GetValue("IdOwner").ToString()));
            });
            _mapper = mapperConfig.CreateMapper();

            _controller = new PropertiesController(_repoMock.Object, _mapper);
        }

        [Test]
        public async Task Get_ReturnsOk_WithItems()
        {
            // arrange
            var doc = new BsonDocument {
                { "IdProperty", "11111111-1111-1111-1111-111111111111" },
                { "Name", "Casa Test" },
                { "Address", "Calle Test" },
                { "Price", 100000 },
                { "Image", "https://img" },
                { "OwnerName", "Owner Test" },
                { "IdOwner", "22222222-2222-2222-2222-222222222222" }
            };
            _repoMock.Setup(r => r.GetPropertiesAsync(It.IsAny<PropertyFilter>(), 1, 20))
                .ReturnsAsync((new List<BsonDocument> { doc }, 1L));

            // act
            var result = await _controller.Get(null, null, null, null,  1, 20) as OkObjectResult;

            // assert
            Assert.IsNotNull(result);
            dynamic value = result.Value;
            Assert.AreEqual(1, (int)value.page);
            Assert.AreEqual(1L, (long)value.totalCount);
            Assert.AreEqual(1, ((IList<PropertyListItemDto>)value.items).Count);
        }
    }
}
