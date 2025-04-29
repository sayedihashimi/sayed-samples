using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace SecurityScanner.Razor.Samples;
public class SqlInjectionController : Controller
{
    public IActionResult GetUser(string username)
    {
        var query = $"SELECT * FROM Users WHERE Username = '{username}'";
        using var conn = new SqlConnection("YourConnectionString");
        using var cmd = new SqlCommand(query, conn);
        conn.Open();
        var reader = cmd.ExecuteReader();
        return Ok();
    }
}
