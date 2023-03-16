using API.Data;
using API.Data.Requests;
using API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("Program");

builder.Services.AddCors();
builder.Services.AddSqlServer<TodoAppDbContext>(builder.Configuration["ConnectionStrings:connectionString"]);
//string connString = builder.Configuration.GetConnectionString("connectionString");
//builder.Services.AddDbContext<TodoAppDbContext>(opt =>
//{
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("connectionString"));
//});
// Add services to the container.
var app = builder.Build();
app.UseCors(opt =>
{
    opt.AllowAnyOrigin();
    opt.AllowAnyMethod();
    opt.AllowAnyHeader();
});

#region Salva a tarefa
app.MapPost("/todos/post", (TodoAppRequest todos, TodoAppDbContext context) =>
{
    app.Logger.LogInformation($"Iniciando processo de post para a tarefa {todos.Descricao}");
    var todo = new TodoApp()
    {
       Descricao = todos.Descricao,
       Data = DateTime.ParseExact(todos.Data, "yyyy-MM-dd", CultureInfo.InvariantCulture),
       Status = todos.Status
    };

    context.TodosApps?.Add(todo);
    context.SaveChanges();
    app.Logger.LogInformation($"Finalizando Post para a tarefa {todos.Descricao} com Código: {todo.Id}");

    return Results.Created($"/todos/{todo.Descricao}", todo.Descricao);
});
#endregion

#region Retorna a lista de TODOS
app.MapGet("/todos/listaTodos", (TodoAppDbContext context) =>
{
    app.Logger.LogInformation("Iniciando processo para listar todas as tarefas.");
    var listTodos = context.TodosApps?
                           .OrderBy(x => x.Descricao)
                           .ToList();
    app.Logger.LogInformation("Listando tarefas...");
    return Results.Json(listTodos);
});
#endregion

#region Busca a tarefa por Descrição
app.MapGet("/todos/{descricao}", ([FromRoute] string descricao, TodoAppDbContext context) =>
{
    app.Logger.LogInformation($"Iniciando processo de busca para a tarefa {descricao}");
    var todo = context.TodosApps?
                      .Include(x => x.Descricao)
                      .Include(x => x.Data)
                      .Include(x => x.Status)
                      .Where(x => x.Descricao == descricao)
                      .First();

    if (todo != null)
    {
        app.Logger.LogInformation($"Mostrando tarefa {descricao}");
        return Results.Ok(todo);
    }

    app.Logger.LogInformation($"Nenhuma tarefa encontrada com a {descricao} digitada.");
    return Results.NotFound();
});
#endregion

#region Busca a tarefa por Status
app.MapGet("/todos/{status}", ([FromRoute] string status, TodoAppDbContext context) =>
{
    app.Logger.LogInformation($"Iniciando processo de busca para o status {status}");
    var todo = context.TodosApps?
                      .Include(x => x.Descricao)
                      .Include(x => x.Data)
                      .Include(x => x.Status)
                      .Where(x => x.Status == status)
                      .First();

    if (todo != null)
    {
        app.Logger.LogInformation($"Mostrando o status {status}");
        return Results.Ok(todo);
    }
    app.Logger.LogInformation($"Não foi localizado nenhum status {status} digitado.");
    return Results.NotFound();
});
#endregion

app.Run();