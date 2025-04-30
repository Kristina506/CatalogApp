using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogApp.Data
{
    public class Product
    {
        public int Id { get; set; } // Основен ключ
        public string Name { get; set; } = null!; // Име на продукта
        public decimal Price { get; set; } // Цена
        public int CategoryId { get; set; } // Външен ключ public 
        public int StockQuantity { get; set; } // Външен ключ
        // Навигационно свойство
        public Category Category { get; set; } = null!;
    }
}
