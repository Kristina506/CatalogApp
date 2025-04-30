using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CatalogApp.Data;

namespace CatalogApp.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductServiceDb_" + System.Guid.NewGuid())
                .Options;
            return new AppDbContext(options);
        }

        [Test]
        public void AddProduct_ShouldAddProduct()
        {
            using var context = GetDbContext();
            var service = new ProductService(context);

            service.AddProduct("Laptop", 1500, 1, 5);

            Assert.AreEqual(1, context.Products.Count());
        }

        [Test]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // уникална база
                .Options;

            using var context = new AppDbContext(options);
            context.Categories.Add(new Category {Id = 1, Name = "Test" }); // нужна връзка
            context.Products.Add(new Product
            {
                Name = "Test Product",
                Price = 100,
                StockQuantity = 10,
                CategoryId = 1
            });
            context.SaveChanges();

            var service = new ProductService(context);

            // Act
            var products = service.GetAllProducts();

            // Assert
            Assert.AreEqual(1, products.Count);
            Assert.AreEqual("Test Product", products[0].Name);
        }


        [Test]
        public void DeleteProduct_ShouldRemoveProduct()
        {
            using var context = GetDbContext();
            var product = new Product { Name = "Tablet", Price = 300, CategoryId = 1, StockQuantity = 15 };
            context.Products.Add(product);
            context.SaveChanges();

            var service = new ProductService(context);
            service.DeleteProduct(product.Id);

            Assert.AreEqual(0, context.Products.Count());
        }
    }
}

