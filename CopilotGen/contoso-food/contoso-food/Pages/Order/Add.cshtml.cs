using contoso_food.Data;
using contoso_food.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class AddToOrderModel : PageModel
{
    private readonly AppDbContext _context;
    public AddToOrderModel(AppDbContext context)
    {
        _context = context;
    }
    [BindProperty]
    public MenuItem? MenuItem { get; set; }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        MenuItem = await _context.MenuItems.FindAsync(id);
        if (MenuItem == null)
            return NotFound();
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id)
    {
        // Add to order logic (session/TempData in real app)
        return RedirectToPage("/Order");
    }
}
