using Microsoft.AspNetCore.Mvc
.RazorPages;

namespace Storefront.Pages;

public class ConfirmationModel
: PageModel
{
  public void OnGet()
  {
    TempData["Basket"] = "[]";
  }
}