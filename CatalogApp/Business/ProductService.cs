using System;
using System.Collections.Generic;
using System.Linq;
using CatalogApp.Data;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    // Създаване на нов продукт
    public void AddProduct(string name, decimal price, int categoryId, int stockQuantity)
    {
        var product = new Product
        {
            Name = name,
            Price = price,
            CategoryId = categoryId,
            StockQuantity = stockQuantity
        };

        _context.Products.Add(product);
        _context.SaveChanges();
    }

    // Извличане на всички продукти
    public List<Product> GetAllProducts()
    {
        return _context.Products
            .Include(p => p.Category)
            .ToList();
    }

    // Извличане на продукт по ID
    public Product GetProductById(int id)
    {
        return _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);
    }

    // Актуализация на продукт
    public void UpdateProduct(int id, string name, decimal price, int categoryId, int stockQuantity)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            product.Name = name;
            product.Price = price;
            product.CategoryId = categoryId;
            product.StockQuantity = stockQuantity;

            _context.SaveChanges();
        }
    }

    // Изтриване на продукт
    public void DeleteProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
