using Microsoft.AspNetCore.Builder;

namespace MyApis
{
    public class MyRootClass
    {
        public static void AddRoutes(WebApplication app)
        {

            var summaries = new[]
            {
                "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/coolstuff", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new CoolStuff
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetCoolStuff")
            .WithOpenApi();
        }
    }

    internal record CoolStuff(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
