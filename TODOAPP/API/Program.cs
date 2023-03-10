using API.Data;
using API.Data.Requests;
using API.Domain;
using API.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using API.Data.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<TodoAppDbContext>(builder.Configuration["ConnectionStrings:connectionString"]);
//string connString = builder.Configuration.GetConnectionString("connectionString");
//builder.Services.AddDbContext<TodoAppDbContext>(opt =>
//{
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("connectionString"));
//});
// Add services to the container.
var app = builder.Build();
var configuration = app.Configuration;
//builder.Services.AddSingleton<IConfiguration>();
//builder.Services.AddTransient<ITodoAppRepository,TodoAppRepository>();

app.MapPost("/todos", (TodoAppRequest todos, TodoAppDbContext context) =>
{
    var todo = new TodoApp()
    {
        Descricao = todos.Descricao,
        Data = DateTime.ParseExact(todos.Data, "dd/MM/yyyy", CultureInfo.InvariantCulture),
        Status = todos.Status
    };

    context.TodosApps.Add(todo);
    context.SaveChanges();

    return Results.Created($"/todos/{todo.Descricao}", todo.Descricao);
});

app.MapGet("/todos/{id}", ([FromRoute] Guid id, TodoAppDbContext context) =>
{
    var todo = context.TodosApps
                      .Include(x => x.Descricao)
                      .Include(x => x.Data)
                      .Include(x => x.Status)
                      .Where(x => x.Id == id)
                      .First();

    if (todo != null)
    {
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.Run();