using Million.Application.Filters;

namespace Million.Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<(IEnumerable<object> Items, long Total)> GetPropertiesAsync(PropertyFilter filter, int page, int pageSize);
        Task SeedSampleDataAsync();
    }
}
