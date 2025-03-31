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

// Добавляем JWT аутентификацию
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Регистрируем сервисы
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<BookEventService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<BranchService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Добавляем middleware для аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
