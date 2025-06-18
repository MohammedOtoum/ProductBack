using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ProductTask.Data;
using ProductTask.Model;
using ProductTask.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register DbContext with Dependency Injection
builder.Services.AddDbContext<DataEF>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IOrderRepository, OrdersRepository>();
builder.Services.AddScoped<ICheckoutFormRepository, CheckoutFormRepository>();

// JWT Bearer Authentication
string? tokenKeyString = builder.Configuration.GetValue<string>("AppSettings:TokenKey");
if (string.IsNullOrEmpty(tokenKeyString))
{
    throw new InvalidOperationException("TokenKey is not set in the configuration.");
}

var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString));
var credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

var tokenValidationParameters = new TokenValidationParameters
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = true,
    ValidateAudience = false,
};

// Add authentication services
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
