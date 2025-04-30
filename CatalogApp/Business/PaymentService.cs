using CatalogApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogApp.Business
{
    public class PaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        // Извършване на плащане по поръчка
        public void MakePayment(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
                throw new Exception("Няма такава поръчка.");

            if (_context.Payments.Any(p => p.OrderId == orderId))
                throw new Exception("Поръчката вече е платена.");

            var payment = new Payment
            {
                OrderId = orderId,
                PaymentDate = DateTime.Now,
                Amount = order.TotalAmount,
                Status = "Платено"
            };

            _context.Payments.Add(payment);

            // Обновяване на статуса на поръчката
            order.Status = "Платено";

            _context.SaveChanges();
        }

        // Преглед на плащане по поръчка
        public Payment GetPaymentByOrderId(int orderId)
        {
            return _context.Payments.FirstOrDefault(p => p.OrderId == orderId);
        }
    }
}
