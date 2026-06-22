using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Storefront.Data;
using System.ComponentModel.DataAnnotations;

namespace Storefront.Pages;

public class OrderLookupModel : PageModel
{
  private readonly ShopContext
    _context;

  public OrderLookupModel(
    ShopContext context)
  {
    _context = context;
  }

  public string ErrorMessage
  { get; set; }
  [BindProperty]
  [Required(
    ErrorMessage =
      "Please enter your " +
      "verification code.")]
  public string VerificationCode
  { get; set; }

  public void OnGet(
    bool notfound = false)
  {
    if (notfound)
    {
      ErrorMessage =
        "We couldn't find an " +
        "order with that " +
        "verification code.";
    }
  }


  public IActionResult OnPost()
  {
    if (!ModelState.IsValid)
    {
      return Page();
    }
    var code = VerificationCode
      .Trim()
      .ToUpperInvariant();

    var order = _context.Orders
      .FirstOrDefault(o =>
        o.VerificationCode ==
          code);

    if (order == null)
    {
      ErrorMessage =
        "We couldn't find an " +
        "order with that " +
        "verification code.";
      return Page();
    }

    return RedirectToPage(
      "OrderDetails",
      new
      {
        code =
        VerificationCode
      });
  }
}