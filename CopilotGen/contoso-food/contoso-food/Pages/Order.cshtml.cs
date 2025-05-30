using contoso_food.Data;
using contoso_food.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OrderModel : PageModel
{
    private readonly AppDbContext _context;
    public OrderModel(AppDbContext context)
    {
        _context = context;
    }
    public IList<MenuItem> OrderItems { get; set; } = new List<MenuItem>();
    public async Task OnGetAsync()
    {
        // For demo: use session or temp data for real order tracking
        OrderItems = new List<MenuItem>();
    }
    public async Task<IActionResult> OnPostRemoveAsync(int id)
    {
        // Remove item from order (session/TempData in real app)
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostSubmitAsync()
    {
        // Submit order logic (save to DB, send notification, etc.)
        TempData["OrderSubmitted"] = true;
        return RedirectToPage();
    }
}
