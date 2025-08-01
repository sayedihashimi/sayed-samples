using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorCSharpSupport.Pages
{
    public class CSharp7Model : PageModel
    {
        public void OnGet()
        {
        }

        // C# 7.0: Generalized async return types - ValueTask
        public async ValueTask<string> GetCachedDataAsync()
        {
            // Simulate a cache check - if data is available, return immediately
            if (DateTime.Now.Millisecond % 2 == 0)
            {
                // Return synchronously without Task allocation
                return "Cached data (synchronous)";
            }
            
            // Simulate async operation
            await Task.Delay(1);
            return "Fresh data (asynchronous)";
        }

        // C# 7.0: Generalized async return types - ValueTask (void equivalent)
        public async ValueTask LogOperationAsync()
        {
            // Simulate quick logging that might be synchronous
            if (DateTime.Now.Millisecond % 3 == 0)
            {
                // Return synchronously
                return;
            }
            
            // Simulate async logging
            await Task.Delay(1);
        }

        // Traditional Task method for comparison
        public async Task<string> GetDataWithTaskAsync()
        {
            await Task.Delay(1);
            return "Data from Task method";
        }
    }
}
