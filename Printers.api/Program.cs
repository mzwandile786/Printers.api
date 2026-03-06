using Microsoft.EntityFrameworkCore;
using Printers.api;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Add services to the container
// =========================

// ✅ Add DbContext for SQL Server
builder.Services.AddDbContext<PrintersDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ✅ Add controllers
builder.Services.AddControllers();

// ✅ Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ JSON cycles handling (optional)
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// =========================
// ✅ ADD CORS POLICY
// =========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// =========================
// Configure the HTTP request pipeline
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ USE CORS (must be before Authorization and MapControllers)
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();