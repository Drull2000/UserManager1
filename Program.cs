using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UserManager.Application.Services;
using UserManager.Application.Validation;
using UserManager.Domain.Entities;
using UserManager.Infrastructure.Persistence;

namespace UserManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        var databasePassword = builder.Configuration["DatabasePassword"];
        if (!string.IsNullOrWhiteSpace(databasePassword))
        {
            connectionString = new SqlConnectionStringBuilder(connectionString)
            {
                Password = databasePassword
            }.ConnectionString;
        }

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

