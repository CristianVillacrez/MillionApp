namespace Million.Application.DTOs
{
    public class PropertyListItemDto
    {
        public string IdProperty { get; set; }
        public string IdOwner { get; set; }
        public string OwnerName { get; set; } 
        public string PropertyName { get; set; }
        public string PropertyAddress { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
