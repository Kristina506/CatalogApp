using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CatalogApp.Data;

public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    // Създаване на нова поръчка от количка
    public Order CreateOrderFromCart(int userId, int cartId)
    {
        var cart = _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c => c.CartId == cartId && c.UserId == userId);

        if (cart == null || cart.CartItems.Count == 0)
        {
            throw new Exception("Количката е празна или не съществува.");
        }

        decimal totalAmount = cart.CartItems
            .Sum(ci => ci.Product.Price * ci.Quantity);

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Status = "Нова",
            TotalAmount = totalAmount,
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in cart.CartItems)
        {
            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product.Price
            });
        }

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cart.CartItems); // Изчистване на количката
        _context.SaveChanges();

        return order;
    }

    // Извличане на поръчки на потребител
    public List<Order> GetOrdersByUser(int userId)
    {
        return _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToList();
    }
}
