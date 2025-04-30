using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CatalogApp.Data;
namespace CatalogApp.Tests
{

    namespace MyApp.Tests
    {
        public class CartServiceTests
        {
            private AppDbContext GetDbContext()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "CartServiceDb_" + System.Guid.NewGuid())
                    .Options;
                return new AppDbContext(options);
            }

            [Test]
            public void CreateCart_ShouldCreateCart()
            {
                using var context = GetDbContext();
                var service = new CartService(context);

                var cart = service.CreateCart(1);

                Assert.IsNotNull(cart);
                Assert.AreEqual(1, context.Carts.Count());
            }

            [Test]
            public void AddProductToCart_ShouldAddItem()
            {
                using var context = GetDbContext();
                context.Products.Add(new Product { Id = 1, Name = "Pen", Price = 2, StockQuantity = 50, CategoryId = 1 });
                context.Carts.Add(new Cart { CartId = 1, UserId = 1, CreatedOn = DateTime.Now });
                context.SaveChanges();

                var service = new CartService(context);
                service.AddProductToCart(1, 1, 3);

                Assert.AreEqual(1, context.CartItems.Count());
            }
        }
    }

}
