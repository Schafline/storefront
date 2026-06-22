using Microsoft.AspNetCore.Mvc.RazorPages;
using Storefront.Models;
using Microsoft.EntityFrameworkCore;
using Storefront.Data;

namespace Storefront.Pages;

public class ProductsModel : PageModel
{
  public List<Product> Products { get; set; } = new();

  private readonly ShopContext _context;

  public ProductsModel(ShopContext context)
  {
    _context = context;
  }

  public async Task OnGetAsync()
  {
    Products =
      await _context.Products.ToListAsync();
  }
}
