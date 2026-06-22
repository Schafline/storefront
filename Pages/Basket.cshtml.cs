using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Storefront.Models;
using Storefront.Data;
using Storefront.Services;

namespace Storefront.Pages;

[IgnoreAntiforgeryToken]
public class BasketModel : PageModel
{
  private readonly IConfiguration _config;
  private readonly BasketService _basketService;
  private readonly ShopContext _context;

  public BasketModel(
    IConfiguration config,
    BasketService basketService,
    ShopContext context)
  {
    _config = config;
    _basketService = basketService;
    _context = context;
  }


  public string PayPalClientId
  { get; private set; }

  public List<Product> BasketItems
  { get; set; } = new();

  [BindProperty]
  public int Id { get; set; }

  public decimal TotalPrice
  { get; set; }

  public IActionResult OnPostRemove()
  {
    var basket = _basketService.GetBasket();
    var itemToRemove =
      basket.FirstOrDefault(p => p.Id == Id);

    if (itemToRemove != null)
    {
      basket.Remove(itemToRemove);
      _basketService.SaveBasket(basket);
    }

    return RedirectToPage();
  }

  public void OnGet()
  {
    BasketItems = _basketService.GetBasket();
    TotalPrice = BasketItems.Sum(p => p.Price);
    PayPalClientId = _config["PayPal:SandboxClientId"];
  }

  public async Task<IActionResult>
    OnPostCompleteOrderAsync(
      [FromBody] PayPalOrderInfo info)
  {
    var cart = _basketService.GetBasket();

    var order = new Order
    {
      OrderDate = DateTime.UtcNow
    };

    foreach (var product in cart)
    {
      order.Items.Add(
        new OrderItem
        {
          ProductId = product.Id,
          Quantity = 1,
          ProductName = product.Name,
          Price = product.Price
        });
    }

    order.Total =
      order.Items.Sum(i => i.Price);
    order.OrderStatus =
      OrderStatusConstants.Paid;
    _context.Orders.Add(order);

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(
        "Database save failed: " +
        ex.Message);
      throw;
    }

    _basketService.Clear();

    return new JsonResult(
      new { orderId = order.Id });
  }

  public class PayPalOrderInfo
  {
    public string OrderId { get; set; }
    public string PayerId { get; set; }
  }

}