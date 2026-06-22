using Storefront.Models;

namespace Storefront.Data;

public static class SeedData
{
  public static void Initialize(ShopContext context)
  {
    // Only seed if the Products table is empty
    if (context.Products.Any())
      return;

    context.Products.AddRange(
        new Product
        {
          Name = "T-shirt",
          Description = "Soft cotton T-shirt",
          Price = 19.99m,
          ImageUrl = "/images/tshirt.jpg"
        },
        new Product
        {
          Name = "Sneakers",
          Description = "Comfortable running shoes",
          Price = 49.99m,
          ImageUrl = "/images/sneakers.jpg"
        }
    );

    context.SaveChanges();
  }
}