using AspireShared;
using Microsoft.EntityFrameworkCore;
using AspireCliReact01.ApiService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspireCliReact01.ApiService.Api;

public static class TaskItemEndpoints
{
    public static void MapTaskItemEndpoints(this RouteGroupBuilder group)
    {
        // GET: api/taskitems
        group.MapGet("/taskitems", async (ApplicationDbContext db) =>
            await db.TaskItems.ToListAsync()
        );

        // GET: api/taskitems/{id}
        group.MapGet("/taskitems/{id}", async (int id, ApplicationDbContext db) =>
            await db.TaskItems.FindAsync(id)
                is TaskItem taskItem ? Results.Ok(taskItem) : Results.NotFound()
        );

        // POST: api/taskitems
        group.MapPost("/taskitems", async (TaskItem taskItem, ApplicationDbContext db) =>
        {
            db.TaskItems.Add(taskItem);
            await db.SaveChangesAsync();
            return Results.Created($"/taskitems/{taskItem.Id}", taskItem);
        });

        // PUT: api/taskitems/{id}
        group.MapPut("/taskitems/{id}", async (int id, TaskItem input, ApplicationDbContext db) =>
        {
            var taskItem = await db.TaskItems.FindAsync(id);
            if (taskItem is null) return Results.NotFound();
            taskItem.Title = input.Title;
            taskItem.Description = input.Description;
            taskItem.IsCompleted = input.IsCompleted;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // DELETE: api/taskitems/{id}
        group.MapDelete("/taskitems/{id}", async (int id, ApplicationDbContext db) =>
        {
            var taskItem = await db.TaskItems.FindAsync(id);
            if (taskItem is null) return Results.NotFound();
            db.TaskItems.Remove(taskItem);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
