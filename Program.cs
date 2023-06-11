using BankManager.Api.Domain.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features?.Get<IExceptionHandlerPathFeature>()?.Error;

    if (exception is ArgumentException)
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

    await context.Response.WriteAsJsonAsync(exception?.Message);
}));

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

var accounts = new List<Account>();


app.MapPost("/accounts", async (CreateAccountVM vm) =>
{
    var account = new Account(vm.Name, vm.InitialBalance);
    accounts.Add(account);

    return Results.Created($"/account/{account.Id}", account);
});

app.MapGet("/accounts", () => Results.Ok(accounts));

app.MapGet("/accounts/{id}", (Guid id) =>
{
    var account = accounts.FirstOrDefault(a => a.Id == id);
    if (account is null)
        return Results.NotFound();

    return Results.Ok(account);
});

app.MapDelete("/accounts/{id}", (Guid id) =>
{
    var account = accounts.FirstOrDefault(a => a.Id == id);
    if (account is null)
        return Results.NotFound();

    accounts.Remove(account);

    return Results.Ok();
});

app.MapPost("/accounts/{id}/deposit", (Guid id, [FromBody] decimal amount) =>
{
    var account = accounts.FirstOrDefault(a => a.Id == id);
    if (account is null)
        return Results.NotFound();

    account.Deposit(amount);

    return Results.Ok(account);
});

app.MapPost("/accounts/{id}/withdraw", (Guid id, [FromBody] decimal amount) =>
{
    var account = accounts.FirstOrDefault(a => a.Id == id);
    if (account is null)
        return Results.NotFound();

    account.Withdraw(amount);

    return Results.Ok(account);
});

app.Run();

public record CreateAccountVM(string Name, decimal InitialBalance);

