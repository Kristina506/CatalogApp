using CatalogApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogApp.Business
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Регистрация на нов потребител
        public void Register(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = password // за по-сигурно приложение тук трябва криптиране
            };

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Проверка за вход
        public User Login(string username, string password)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Извличане на всички потребители (ако трябва)
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
