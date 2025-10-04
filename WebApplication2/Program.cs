using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication2.Services.Book;
using WebApplication2.Services.BookEvent;
using WebApplication2.Services.Dashboard;
using WebApplication2.Services.Auth;
using WebApplication2.Services.Author;
using WebApplication2.Services.Category;
using WebApplication2.Services.Branch;
using WebApplication2.Services.File;
using Microsoft.Extensions.FileProviders;
using WebApplication2.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(7200, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

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

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<BookEventService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<BranchService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<BookEventReportService>();

var app = builder.Build();

// Global exception middleware - MUST be first in pipeline
app.UseMiddleware<GlobalExceptionMiddleware>();

var coverLinkPath = Path.Combine(builder.Environment.ContentRootPath, "coverlink");
if (!Directory.Exists(coverLinkPath))
{
    Directory.CreateDirectory(coverLinkPath);
}

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(coverLinkPath),
    RequestPath = "/coverlink"
});

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

var port = 7200;
var hostName = System.Net.Dns.GetHostName();
var ips = System.Net.Dns.GetHostAddresses(hostName)
    .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
    .Select(ip => ip.ToString())
    .ToList();

Console.WriteLine($"\nAPI запущен на порту: {port}");
Console.WriteLine("Доступные IP-адреса:");
foreach (var ip in ips)
{
    Console.WriteLine($"https://{ip}:{port}");
}
Console.WriteLine($"\nSwagger UI доступен по адресу: https://localhost:{port}/swagger\n");

app.Run();
