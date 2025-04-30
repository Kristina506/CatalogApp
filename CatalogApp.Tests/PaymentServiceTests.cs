using CatalogApp.Business;
using CatalogApp.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogApp.Tests
{
    public class PaymentServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PaymentServiceDb_" + System.Guid.NewGuid())
                .Options;
            return new AppDbContext(options);
        }

        [Test]
        public void MakePayment_ShouldCreatePayment()
        {
            using var context = GetDbContext();
            var order = new Order { UserId = 1, OrderDate = DateTime.Now, TotalAmount = 100, Status = "Нова" };
            context.Orders.Add(order);
            context.SaveChanges();

            var service = new PaymentService(context);
            service.MakePayment(order.OrderId);

            Assert.AreEqual(1, context.Payments.Count());
        }

        [Test]
        public void GetPaymentByOrderId_ShouldReturnPayment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);

            var order = new Order
            {
                UserId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 150,
                Status = "Нова"
            };
            context.Orders.Add(order);
            context.SaveChanges(); // нужно, за да се генерира OrderId

            var payment = new Payment
            {
                OrderId = order.OrderId,
                Amount = 150,
                Status = "Платено",
                PaymentDate = DateTime.Now
            };
            context.Payments.Add(payment);
            context.SaveChanges();

            var service = new PaymentService(context);

            // Act
            var result = service.GetPaymentByOrderId(order.OrderId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(order.OrderId, result.OrderId);
            Assert.AreEqual("Платено", result.Status);
        }

    }
}
