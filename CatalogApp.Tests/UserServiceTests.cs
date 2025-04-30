using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CatalogApp.Business;
using CatalogApp.Data;

namespace CatalogApp.Tests
{
    public class UserServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserServiceDb_" + System.Guid.NewGuid())
                .Options;
            return new AppDbContext(options);
        }

        [Test]
        public void Register_ShouldCreateUser()
        {
            using var context = GetDbContext();
            var service = new UserService(context);

            service.Register("user1", "user1@mail.com", "pass123");

            Assert.AreEqual(1, context.Users.Count());
        }

        [Test]
        public void Login_ShouldReturnUser_WhenCorrect()
        {
            using var context = GetDbContext();
            context.Users.Add(new User { Username = "test", Email = "t@mail.com", Password = "1234" });
            context.SaveChanges();

            var service = new UserService(context);
            var user = service.Login("test", "1234");

            Assert.IsNotNull(user);
        }

        [Test]
        public void Login_ShouldReturnNull_WhenWrong()
        {
            using var context = GetDbContext();
            context.Users.Add(new User { Username = "test", Email = "t@mail.com", Password = "1234" });
            context.SaveChanges();

            var service = new UserService(context);
            var user = service.Login("test", "wrongpass");

            Assert.IsNull(user);
        }
    }
}
