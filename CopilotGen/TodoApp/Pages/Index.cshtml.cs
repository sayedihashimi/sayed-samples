using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Models;
using System.Collections.Generic;

namespace TodoApp.Pages;

public class IndexModel : PageModel
{
    private static List<TodoItem> _todoItems = new();
    private static int _nextId = 1;

    [BindProperty]
    public string? NewItemTitle { get; set; }

    public List<TodoItem> TodoItems => _todoItems;

    public void OnGet()
    {
    }

    public IActionResult OnPostAdd()
    {
        if (!string.IsNullOrWhiteSpace(NewItemTitle))
        {
            _todoItems.Add(new TodoItem { Id = _nextId++, Title = NewItemTitle.Trim() });
        }
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        var item = _todoItems.Find(x => x.Id == id);
        if (item != null)
        {
            _todoItems.Remove(item);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostMoveUp(int id)
    {
        int idx = _todoItems.FindIndex(x => x.Id == id);
        if (idx > 0)
        {
            var temp = _todoItems[idx - 1];
            _todoItems[idx - 1] = _todoItems[idx];
            _todoItems[idx] = temp;
        }
        return RedirectToPage();
    }

    public IActionResult OnPostMoveDown(int id)
    {
        int idx = _todoItems.FindIndex(x => x.Id == id);
        if (idx >= 0 && idx < _todoItems.Count - 1)
        {
            var temp = _todoItems[idx + 1];
            _todoItems[idx + 1] = _todoItems[idx];
            _todoItems[idx] = temp;
        }
        return RedirectToPage();
    }
}
