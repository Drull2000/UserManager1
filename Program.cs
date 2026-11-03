using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using UserManager.Data;
using UserManager.Mapping;
using UserManager.Services;

namespace UserManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // FluentValidation registration (from Classwork 24.10.2026)
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        var databasePassword = builder.Configuration["DatabasePassword"]
            ?? throw new InvalidOperationException(
                "Database password is not configured. Set it as User Secret 'DatabasePassword' or environment variable 'DatabasePassword'.");

        var sqlConnectionString = new SqlConnectionStringBuilder(connectionString)
        {
            Password = databasePassword
        }.ConnectionString;

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(sqlConnectionString));

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(MappingProfile).Assembly);
        });

        builder.Services.AddScoped<IProductService, ProductService>();

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

