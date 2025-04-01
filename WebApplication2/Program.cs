using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication2.Services;
using WebApplication2.Services.Book;
using WebApplication2.Services.BookEvent;
using WebApplication2.Services.Dashboard;
using WebApplication2.Services.Auth;
using WebApplication2.Services.Author;
using WebApplication2.Services.Category;
using WebApplication2.Services.Branch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Настройка JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Регистрация сервисов
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<BookEventService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<BranchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

// Получаем информацию о порте и IP
var kestrelUrl = builder.Configuration["Kestrel:Endpoints:Http:Url"];
var port = 5000; // порт по умолчанию
if (!string.IsNullOrEmpty(kestrelUrl))
{
    var uri = new Uri(kestrelUrl);
    port = uri.Port;
}

var hostName = System.Net.Dns.GetHostName();
var ips = System.Net.Dns.GetHostAddresses(hostName)
    .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
    .Select(ip => ip.ToString())
    .ToList();

Console.WriteLine($"\nAPI запущен на порту: {port}");
Console.WriteLine("Доступные IP-адреса:");
foreach (var ip in ips)
{
    Console.WriteLine($"http://{ip}:{port}");
}
Console.WriteLine($"\nSwagger UI доступен по адресу: http://localhost:{port}/swagger\n");

app.Run();
