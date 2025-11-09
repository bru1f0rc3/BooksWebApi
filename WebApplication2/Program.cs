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
using WebApplication2.Services.Email;
using WebApplication2.Interfaces;
using Microsoft.Extensions.FileProviders;

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

// Регистрация сервисов через интерфейсы (Dependency Injection)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookEventService, BookEventService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IBookEventReportService, BookEventReportService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

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
