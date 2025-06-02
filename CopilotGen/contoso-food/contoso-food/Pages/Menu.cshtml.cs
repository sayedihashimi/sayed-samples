using contoso_food.Data;
using contoso_food.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MenuModel : PageModel
{
    private readonly AppDbContext _context;
    public MenuModel(AppDbContext context)
    {
        _context = context;
    }
    public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public async Task OnGetAsync()
    {
        MenuItems = await _context.MenuItems.AsNoTracking().ToListAsync();
    }
}
