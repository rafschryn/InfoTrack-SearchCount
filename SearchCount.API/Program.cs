using Microsoft.EntityFrameworkCore;
using SearchCount.API;
using SearchCount.API.RequestValidations;
using SearchCount.Contexts;
using SearchCount.Handlers;
using SearchCount.Handlers.Interfaces;
using SearchCount.Repositories;
using SearchCount.Repositories.Interfaces;
using SearchCount.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHttpClient<SearchCountHandler>();
builder.Services.AddScoped<ISearchCountHandler, SearchCountHandler>();
builder.Services.AddScoped<ISearchCountHistoryHandler, SearchCountHistoryHandler>();
builder.Services.AddScoped<IValidator<SearchCountRequest>, SearchCountRequestValidator>();
builder.Services.AddScoped<ISearchCountHistoryRepository, SearchCountHistoryRepository>();

builder.Services.AddDbContext<SearchCountContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SearchCountContext")));

var app = builder.Build();

//
app.UseStatusCodePages();
app.UseExceptionHandler();
app.UseCors("AllowSpecificOrigin");
//

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
