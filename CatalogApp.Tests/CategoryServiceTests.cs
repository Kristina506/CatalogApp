using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CatalogApp.Data;

namespace CatalogApp.Tests
{
    public class CategoryServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryServiceDb_" + System.Guid.NewGuid())
                .Options;
            return new AppDbContext(options);
        }

        [Test]
        public void AddCategory_ShouldAddCategory()
        {
            using var context = GetDbContext();
            var service = new CategoryService(context);

            service.AddCategory("Electronics");

            Assert.AreEqual(1, context.Categories.Count());
        }

        [Test]
        public void GetAllCategories_ShouldReturnAll()
        {
            using var context = GetDbContext();
            context.Categories.Add(new Category { Name = "Books" });
            context.SaveChanges();

            var service = new CategoryService(context);
            var categories = service.GetAllCategories();

            Assert.AreEqual(1, categories.Count);
        }

        [Test]
        public void UpdateCategory_ShouldChangeName()
        {
            using var context = GetDbContext();
            var category = new Category { Name = "Old Name" };
            context.Categories.Add(category);
            context.SaveChanges();

            var service = new CategoryService(context);
            service.UpdateCategory(category.Id, "New Name");

            Assert.AreEqual("New Name", context.Categories.Find(category.Id).Name);
        }

        [Test]
        public void DeleteCategory_ShouldRemoveCategory()
        {
            using var context = GetDbContext();
            var category = new Category { Name = "Delete Me" };
            context.Categories.Add(category);
            context.SaveChanges();

            var service = new CategoryService(context);
            service.DeleteCategory(category.Id);

            Assert.AreEqual(0, context.Categories.Count());
        }
    }
}
