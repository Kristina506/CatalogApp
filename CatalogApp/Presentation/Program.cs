using CatalogApp.Business;
using CatalogApp.Data;
class Program
{
    static void Main(string[] args)
    {
        using var context = new AppDbContext();
        var productService = new ProductService(context);
        var categoryService = new CategoryService(context);
        var userService = new UserService(context);
        var cartService = new CartService(context);
        Cart currentCart = null; // ще пазим активната количка
        User currentUser = null; // ще пазим текущия потребител
        var orderService = new OrderService(context);
        var paymentService = new PaymentService(context);

        while (true)
        {
            Console.WriteLine("\nГлавно Меню:");
            Console.WriteLine("1. Управление на продукти");
            Console.WriteLine("2. Управление на категории");
            Console.WriteLine("3. Управление на потребители");
            Console.WriteLine("4. Управление на количка");
            Console.WriteLine("5. Поръчки"); 
            Console.WriteLine("6. Плащания");
            Console.WriteLine("7. Изход");

            var mainChoice = Console.ReadLine();

            switch (mainChoice)
            {
                case "1":
                    ManageProducts(productService);
                    break;
                case "2":
                    ManageCategories(categoryService);
                    break;
                case "3":
                    currentUser = ManageUsers(userService);
                    if (currentUser != null)
                    {
                        currentCart = cartService.CreateCart(currentUser.UserId);
                    }
                    break;
                case "4":
                    if (currentCart != null)
                        ManageCart(cartService, currentCart);
                    else
                        Console.WriteLine("Моля, влез в профил първо!");
                    break;
                case "5":
                    if (currentUser != null && currentCart != null)
                        ManageOrders(orderService, currentUser.UserId, currentCart);
                    else
                        Console.WriteLine("Моля, влез в профил и създай количка първо!");
                    break;
                case "6":
                    if (currentUser != null)
                        ManagePayments(paymentService, currentUser.UserId);
                    else
                        Console.WriteLine("Моля, влез в профил първо!");
                    break;
            }
        }
    }

    static void ManageProducts(ProductService productService)
    {
        Console.WriteLine("\nМеню - Продукти:");
        Console.WriteLine("1. Добави продукт");
        Console.WriteLine("2. Покажи всички продукти");
        Console.WriteLine("3. Актуализирай продукт");
        Console.WriteLine("4. Изтрий продукт");
        Console.WriteLine("5. Назад");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Име: ");
                string name = Console.ReadLine();
                Console.Write("Цена: ");
                decimal price = decimal.Parse(Console.ReadLine());
                Console.Write("Категория Id: ");
                int categoryId = int.Parse(Console.ReadLine());
                Console.Write("Количество: ");
                int quantity = int.Parse(Console.ReadLine());

                productService.AddProduct(name, price, categoryId, quantity);
                Console.WriteLine("Продуктът е добавен.");
                break;

            case "2":
                var products = productService.GetAllProducts();
                foreach (var p in products)
                {
                    Console.WriteLine($"{p.Id}. {p.Name} - {p.Price}лв - Категория: {p.Category?.Name}");
                }
                break;

            case "3":
                Console.Write("ID на продукта за обновяване: ");
                int updateId = int.Parse(Console.ReadLine());
                Console.Write("Ново име: ");
                string newName = Console.ReadLine();
                Console.Write("Нова цена: ");
                decimal newPrice = decimal.Parse(Console.ReadLine());
                Console.Write("Нова категория Id: ");
                int newCategoryId = int.Parse(Console.ReadLine());
                Console.Write("Ново количество: ");
                int newQuantity = int.Parse(Console.ReadLine());

                productService.UpdateProduct(updateId, newName, newPrice, newCategoryId, newQuantity);
                Console.WriteLine("Продуктът е обновен.");
                break;

            case "4":
                Console.Write("ID на продукта за изтриване: ");
                int deleteId = int.Parse(Console.ReadLine());
                productService.DeleteProduct(deleteId);
                Console.WriteLine("Продуктът е изтрит.");
                break;
        }
    }

    static void ManageCategories(CategoryService categoryService)
    {
        Console.WriteLine("\nМеню - Категории:");
        Console.WriteLine("1. Добави категория");
        Console.WriteLine("2. Покажи всички категории");
        Console.WriteLine("3. Актуализирай категория");
        Console.WriteLine("4. Изтрий категория");
        Console.WriteLine("5. Назад");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Име на категория: ");
                string name = Console.ReadLine();
                categoryService.AddCategory(name);
                Console.WriteLine("Категорията е добавена.");
                break;

            case "2":
                var categories = categoryService.GetAllCategories();
                foreach (var c in categories)
                {
                    Console.WriteLine($"{c.Id}. {c.Name}");
                }
                break;

            case "3":
                Console.Write("ID на категорията за обновяване: ");
                int updateId = int.Parse(Console.ReadLine());
                Console.Write("Ново име: ");
                string newName = Console.ReadLine();
                categoryService.UpdateCategory(updateId, newName);
                Console.WriteLine("Категорията е обновена.");
                break;

            case "4":
                Console.Write("ID на категорията за изтриване: ");
                int deleteId = int.Parse(Console.ReadLine());
                categoryService.DeleteCategory(deleteId);
                Console.WriteLine("Категорията е изтрита.");
                break;
        }
    }

    static User ManageUsers(UserService userService)
    {
        Console.WriteLine("\nМеню - Потребители:");
        Console.WriteLine("1. Регистрация");
        Console.WriteLine("2. Вход");
        Console.WriteLine("3. Назад");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Потребителско име: ");
                string username = Console.ReadLine();
                Console.Write("Имейл: ");
                string email = Console.ReadLine();
                Console.Write("Парола: ");
                string password = Console.ReadLine();

                userService.Register(username, email, password);
                Console.WriteLine("Регистрация успешна.");
                return null;

            case "2":
                Console.Write("Потребителско име: ");
                string loginUsername = Console.ReadLine();
                Console.Write("Парола: ");
                string loginPassword = Console.ReadLine();

                var user = userService.Login(loginUsername, loginPassword);
                if (user != null)
                {
                    Console.WriteLine($"Добре дошъл, {user.Username}!");
                    return user;
                }
                else
                {
                    Console.WriteLine("Грешно име или парола.");
                    return null;
                }

            default:
                return null;
        }
    }


    static void ManageCart(CartService cartService, Cart currentCart)
    {
        Console.WriteLine("\nМеню - Количка:");
        Console.WriteLine("1. Добави продукт");
        Console.WriteLine("2. Покажи количката");
        Console.WriteLine("3. Назад");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("ID на продукт: ");
                int productId = int.Parse(Console.ReadLine());
                Console.Write("Количество: ");
                int quantity = int.Parse(Console.ReadLine());

                cartService.AddProductToCart(currentCart.CartId, productId, quantity);
                Console.WriteLine("Продуктът е добавен.");
                break;

            case "2":
                var cart = cartService.GetCartWithItems(currentCart.CartId);
                Console.WriteLine($"Количка ID: {cart.CartId}, Създадена на: {cart.CreatedOn}");

                foreach (var item in cart.CartItems)
                {
                    Console.WriteLine($"- {item.Product.Name} x {item.Quantity} = {item.Product.Price * item.Quantity}лв");
                }
                break;
        }
    }

    static void ManageOrders(OrderService orderService, int userId, Cart currentCart)
    {
        Console.WriteLine("\nМеню - Поръчки:");
        Console.WriteLine("1. Направи поръчка от количката");
        Console.WriteLine("2. Покажи всички поръчки");
        Console.WriteLine("3. Назад");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                try
                {
                    var order = orderService.CreateOrderFromCart(userId, currentCart.CartId);
                    Console.WriteLine($"Поръчката е създадена успешно. Обща сума: {order.TotalAmount}лв");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Грешка: {ex.Message}");
                }
                break;

            case "2":
                var orders = orderService.GetOrdersByUser(userId);
                foreach (var order in orders)
                {
                    Console.WriteLine($"\nПоръчка #{order.OrderId} - Дата: {order.OrderDate}, Статус: {order.Status}, Обща сума: {order.TotalAmount}лв");
                    foreach (var item in order.OrderItems)
                    {
                        Console.WriteLine($"- {item.Product.Name} x {item.Quantity} = {item.Price * item.Quantity}лв");
                    }
                }
                break;
        }
    }

    static void ManagePayments(PaymentService paymentService, int userId)
    {
        Console.WriteLine("\nМеню - Плащане:");
        Console.WriteLine("1. Извърши плащане по поръчка");
        Console.WriteLine("2. Преглед на плащане");
        Console.WriteLine("3. Назад");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Въведи ID на поръчка: ");
                int orderId = int.Parse(Console.ReadLine());

                try
                {
                    paymentService.MakePayment(orderId);
                    Console.WriteLine("Плащането е успешно!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Грешка: {ex.Message}");
                }
                break;

            case "2":
                Console.Write("Въведи ID на поръчка: ");
                int orderIdToCheck = int.Parse(Console.ReadLine());
                var payment = paymentService.GetPaymentByOrderId(orderIdToCheck);

                if (payment != null)
                {
                    Console.WriteLine($"Плащане №{payment.PaymentId} за поръчка #{payment.OrderId} на стойност {payment.Amount}лв, статус: {payment.Status}, дата: {payment.PaymentDate}");
                }
                else
                {
                    Console.WriteLine("Няма плащане за тази поръчка.");
                }
                break;
        }
    }

}
