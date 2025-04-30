// Services/CartService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using CatalogApp.Data;
using Microsoft.EntityFrameworkCore;

public class CartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    // Създаване на нова количка за потребител
    public Cart CreateCart(int userId)
    {
        var cart = new Cart
        {
            UserId = userId,
            CreatedOn = DateTime.Now
        };

        _context.Carts.Add(cart);
        _context.SaveChanges();

        return cart;
    }

    // Добавяне на продукт към количка
    public void AddProductToCart(int cartId, int productId, int quantity)
    {
        var existingItem = _context.CartItems
            .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var newItem = new CartItem
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity
            };

            _context.CartItems.Add(newItem);
        }

        _context.SaveChanges();
    }

    // Извличане на количка с нейните продукти
    public Cart GetCartWithItems(int cartId)
    {
        return _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c => c.CartId == cartId);
    }
}
