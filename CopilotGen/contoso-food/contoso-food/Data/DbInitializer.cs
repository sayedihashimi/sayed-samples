using contoso_food.Models;

namespace contoso_food.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.MenuItems.Any())
                return;

            var items = new MenuItem[]
            {
                new MenuItem { Name = "Pancakes", Description = "Fluffy pancakes served with syrup", Price = 6.99M, Category = "Breakfast" },
                new MenuItem { Name = "Scrambled Eggs", Description = "Eggs cooked to order", Price = 4.99M, Category = "Breakfast" },
                new MenuItem { Name = "Bacon", Description = "Crispy bacon strips", Price = 3.99M, Category = "Breakfast" },
                new MenuItem { Name = "Cheeseburger", Description = "Beef patty with cheese, lettuce, tomato, and onion", Price = 9.99M, Category = "Lunch" },
                new MenuItem { Name = "Club Sandwich", Description = "Turkey, bacon, lettuce, tomato, and mayo on toasted bread", Price = 8.99M, Category = "Lunch" },
                new MenuItem { Name = "Coffee", Description = "Freshly brewed coffee", Price = 2.49M, Category = "Beverage" }
            };
            context.MenuItems.AddRange(items);
            context.SaveChanges();
        }
    }
}
