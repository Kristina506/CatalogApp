using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using CatalogApp.Data;
namespace CatalogApp.Tests
{
    namespace CatalogApp.Tests
    {
        public class OrderServiceTests
        {
            private AppDbContext GetDbContext()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: "OrderServiceDb_" + System.Guid.NewGuid())
                    .Options;
                return new AppDbContext(options);
            }

            [Test]
            public void CreateOrderFromCart_ShouldCreateOrder()
            {
                using var context = GetDbContext();
                var user = new User { Username = "User1", Email = "u@mail.com", Password = "1234" };
                var product = new Product { Id = 1, Name = "Book", Price = 20, StockQuantity = 30, CategoryId = 1 };
                context.Users.Add(user);
                context.Products.Add(product);
                context.SaveChanges();

                var cart = new Cart { UserId = user.UserId, CreatedOn = DateTime.Now };
                context.Carts.Add(cart);
                context.SaveChanges();

                context.CartItems.Add(new CartItem { CartId = cart.CartId, ProductId = product.Id, Quantity = 2 });
                context.SaveChanges();

                var service = new OrderService(context);
                var order = service.CreateOrderFromCart(user.UserId, cart.CartId);

                Assert.AreEqual(1, context.Orders.Count());
                Assert.AreEqual(2, order.OrderItems.First().Quantity);
            }
        }
    }

}
