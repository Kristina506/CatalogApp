using System;
using System.Collections.Generic;
using System.Linq;
using CatalogApp.Data;

public class CategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    // Създаване на нова категория
    public void AddCategory(string name)
    {
        var category = new Category
        {
            Name = name
        };

        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    // Връщане на всички категории
    public List<Category> GetAllCategories()
    {
        return _context.Categories.ToList();
    }

    // Намиране на категория по ID
    public Category GetCategoryById(int id)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }

    // Обновяване на категория
    public void UpdateCategory(int id, string newName)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            category.Name = newName;
            _context.SaveChanges();
        }
    }

    // Изтриване на категория
    public void DeleteCategory(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
