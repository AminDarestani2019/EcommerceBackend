using Domain.Entities.ProductEntity;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.SeedData
{
    public class GenerateFakeData
    {
        public static async Task SeedDataAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
			try
			{
				if (!await context.ProductBrand.AnyAsync())
				{
                    var brands = ProductBrands();
                    await context.ProductBrand.AddRangeAsync(brands);
                    await context.SaveChangesAsync();
                }
				if (!await context.ProductType.AnyAsync())
				{
                    var types = ProductTypes();
                    await context.ProductType.AddRangeAsync(types);
                    await context.SaveChangesAsync();
                }
				if (!await context.Products.AnyAsync())
				{
                    //TODO picture url
                    var products = Products();
                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }
			}
			catch (Exception)
			{

				throw;
			}
        }
        private static List<ProductBrand> ProductBrands()
        {
            var brands = new List<ProductBrand>()
        {
            new()
            {
                Description =
                    "Samsung is a global leader in technology, opening new possibilities .\r\n",
                Summary =
                    "Our story begins in 1969, when Samsung was created for the purpose .",
                Title = "Samsung",
            },
            new()
            {
                Description =
                    "Apple Inc. designs, manufactures and markets smartphones,.\r\n",
                Summary =
                    "Its product categories include iPhone, Mac, iPad, and Wearables.",
                Title = "Apple",
            },
            new()
            {
                Description =
                    "manufacturer of consumer electronics and related software, home appliances.\r\n",
                Summary =
                    "Xiaomi is a consumer electronics and smart manufacturing company with smartphones .",
                Title = "Xiaomi",
            },
            new()
            {
                Description =
                    "Founded in 1987, Huawei is a leading global provider of information and communications technology (ICT) infrastructure and smart devices.\r\n",
                Summary =
                    "It designs, develops, manufactures and sells telecommunications equipment, consumer electronics.",
                Title = "Huawei",
            },
            new()
            {
                Description =
                    "The company's product portfolio consists of virtual reality systems, virtual reality software \r\n",
                Summary =
                    "HTC Corporation designs, manufactures, assembles, processes.",
                Title = "HTC",
            },
            new()
            {
                Description =
                    "Google LLC (Google), a subsidiary of Alphabet Inc, is a provider of search and advertising.   \r\n",
                Summary =
                    "an American multinational corporation and technology company focusing on online advertising.",
                Title = "Google",
            },
            new()
            {
                Description =
                    "ZTE Corp (ZTE) provides telecommunications, enterprise, and innovative technologies and solutions for the mobile internet.\r\n",
                Summary =
                    "ZTE Corporation is a Chinese partially state-owned technology company .",
                Title = "ZTE",
            }
        };
            return brands;
        }
        private static List<ProductType> ProductTypes()
        {
            var types = new List<ProductType>()
        {
            new()
            {
                Description =
                    "A cellphone is simply a telephone that doesn't need a landline connection. \r\n",
                Summary =
                    "A cell phone is a portable electronic device",
                Title = "Cellphone",
            },
            new()
            {
                Description =
                    "Tablets are portable, touchscreen devices that are larger .\r\n",
                Summary =
                    "A tablet is a wireless, portable personal computer. ",
                Title = "Tablet",
            }
        };
            return types;
        }
        private static IEnumerable<Product> Products()
        {
            var products = new List<Product>()
        {
            new()
            {
                Description =
                    "Product Description Honor X8A - cyan lake - 4G smartphone - 128 GB - GSM Product type 4G smartphone Display LCD display.\r\n",
                Summary = " Honor X8A - cyan lake - 4G smartphone",
                PictureUrl = "HonorX8A.jpg",
                Price = 140,
                Title = "Honor X8a Dual SIM ",
                ProductBrandId = 4,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "Your new superpower. A superbright display in a durable design. Hollywood-worthy video shooting made easy. \r\n",
                Summary = "iPhone 13 Pro Max A2644",
                PictureUrl = "iphone13.jpg",
                Price = 587,
                Title = "iPhone 13 Pro Max A2644",
                ProductBrandId = 2,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "NOTE: Global Version. No Warranty. This device is globally unlocked and ready to be used with your preferred GSM Carrier.\r\n",
                Summary = "HTC U23 Pro 5G Dual 256GB 8GB RAM",
                PictureUrl = "htcu23.jpg",
                Price = 399,
                Title = "HTC U23 Pro 5G Dual ",
                ProductBrandId = 5,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "Honor X6 6.5\" Dual SIM | GSM Factory Unlocked | 50MP Triple Camera | 5000mAh | 4GB+64GB | Android 12 | GSM Only .\r\n",
                Summary = "Honor X6 6.5\" Dual SIM",
                PictureUrl = "honorx6a.jpg",
                Price = 149,
                Title = "Honor X6 6.5\" Dual SIM",
                ProductBrandId = 4,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "Xiaomi Poco M6 Pro 4G LTE GSM (256GB + 8GB) 64MP Triple Camera 6.67\" Octa Core (Tmobile Mint Tello Global) Unlocked .\r\n",
                Summary = "Xiaomi Poco M6 Pro 4G LTE GSM (256GB + 8GB)",
                PictureUrl = "pocom6.jpg",
                Price = 189,
                Title = "Xiaomi Poco M6 ",
                ProductBrandId = 3,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "Honor X8a (CRT-LX3) 256GB+8GB RAM | 4500mAh Battery | 4G LTE | 6.7\" 90Hz IPS LCD Display | Dual SIM | 100MP Camera .\r\n",
                Summary = "Honor X8a (CRT-LX3) 256GB+8GB RAM ",
                PictureUrl = "honorx8A.jpg",
                Price = 219,
                Title = "Honor X8a (CRT-LX3)",
                ProductBrandId = 4,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "SAMSUNG Galaxy S24 Ultra Cell Phone, 256GB AI Smartphone, Unlocked Android, 200MP, 100x Zoom Cameras.\r\n",
                Summary = "SAMSUNG Galaxy S24 Ultra Cell Phone",
                PictureUrl = "s24.jpg",
                Price = 1289,
                Title = "SAMSUNG Galaxy S24",
                ProductBrandId = 2,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "Xiaomi 14 Ultra 5G + 4G LTE (512GB + 16GB) Global ROM Unlocked Worldwide (ONLY Tmobile Mint Tello & Global).\r\n",
                Summary = "Xiaomi 14 Ultra 5G + 4G LTE (512GB + 16GB)",
                PictureUrl = "xiaomi14.jpg",
                Price = 1239,
                Title = "Xiaomi 14 Ultra 5G",
                ProductBrandId = 3,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "nubia Z60 Ultra 5G Unlocked Cellphone - Android Smartphone with UDC Tech, Snapdragon 8 Gen 3 Chips.\r\n",
                Summary = "nubia Z60 Ultra 5G Unlocked Cellphone.",
                PictureUrl = "nubia.jpg",
                Price = 699,
                Title = "nubia Z60 Ultra 5G ",
                ProductBrandId = 7,
                ProductTypeId = 1,
            },
            new()
            {
                Description =
                    "Google Pixel 8 Pro - Unlocked Android Smartphone with Telephoto Lens and Super Actua Display - 24-Hour Battery - Obsidian - 128 GB.\r\n",
                Summary = "Google Pixel 8 Pro",
                PictureUrl = "googlepixel.jpg",
                Price = 316,
                Title = "Google Pixel 8 Pro",
                ProductBrandId = 6,
                ProductTypeId = 1,
            }
        };
            return products;
        }
    }
}
