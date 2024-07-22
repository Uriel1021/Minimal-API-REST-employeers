using Microsoft.EntityFrameworkCore;
using MinimalAPIPostgresSQL.Data;
using MinimalAPIPostgresSQL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Coneccion a base de datos
var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
//Inyectar
builder.Services.AddDbContext<OfficeDb>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/employees/", async (Employer e, OfficeDb db) =>
{
    db.Employers.Add(e);
    await db.SaveChangesAsync();

    return Results.Created($"/employee/{e.Id}", e);
});

app.MapGet("/employees/{id:int}", async (int id, OfficeDb db) =>
{
    return await db.Employers.FindAsync(id) is Employer e ? Results.Ok(e) : Results.NotFound();
});

app.MapGet("/employees", async (OfficeDb db) => await db.Employers.ToListAsync());

app.MapPut("/employees/{id:int}", async (int id, Employer e, OfficeDb db) =>
{
    if (e.Id != id)
        return Results.BadRequest();

    var employee = await db.Employers.FindAsync(id);

    if (employee is null) return Results.NotFound();

    employee.Description = e.Description;
    employee.City = e.City;
    employee.Name = e.Name;
    employee.Address = e.Address;

    await db.SaveChangesAsync();

    return Results.Ok(employee);
});


app.MapDelete("/employee/{id:int}", async (int id, OfficeDb db) =>
{
    var employee = await db.Employers.FindAsync(id);

    if (employee is null) return Results.NotFound();

    db.Employers.Remove(employee);
    await db.SaveChangesAsync();
    
    return Results.NoContent();

});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
