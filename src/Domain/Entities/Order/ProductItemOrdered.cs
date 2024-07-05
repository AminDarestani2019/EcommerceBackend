namespace Domain.Entities.Order
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered(int productItemId, string productName, string productBrandName, string productTypeName, string productUrl)
        {
            ProductBrandName = productBrandName;
            ProductTypeName = productTypeName;
            PictureUrl = productUrl;
            ProductItemId = productItemId;
            ProductName = productName;
        }
        public ProductItemOrdered()
        {

        }
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductBrandName { get; set; }
        public string  ProductTypeName { get; set; }
        public string PictureUrl { get; set; }
    }
}
