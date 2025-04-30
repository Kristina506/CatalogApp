using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogApp.Data
{
    public class Category
    {
        public int Id { get; set; } // Основен ключ
        public string Name { get; set; } = null!; // Име на категорията

        // Навигационно свойство - много продукти
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
