using BooksApi.Controllers.BookEventController;
using BooksApi.Service.BookEventService;
using BooksApi.Service.BookService;
using BooksApi.Service.DashboardService;
using BooksApi.Service;
using BooksApi.Service.BookManagement;
using Supabase;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<Supabase.Client>(provider =>
{
    var url = "https://uvobqbanbbtrbnmsghxb.supabase.co";
    var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InV2b2JxYmFuYmJ0cmJubXNnaHhiIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjkxNzM4MzUsImV4cCI6MjA0NDc0OTgzNX0.ng171iYkBN7SnV4pSyIquzJkwNvK_6NMqEFcREY0Ctc";
    var options = new SupabaseOptions { AutoConnectRealtime = true };
    var client = new Supabase.Client(url, key, options);
    client.InitializeAsync().Wait();
    return client;
});

builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<UserRegistrationForm>(provider =>
{
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new UserRegistrationForm(supabaseClient);
});

builder.Services.AddScoped<UserAuthForm>(provider =>
{
    var supabaseClient = provider.GetService<Supabase.Client>();
    var jwtService = provider.GetService<JwtService>();
    return new UserAuthForm(supabaseClient, jwtService);
});

builder.Services.AddScoped<ListedBookService>(provider =>
{
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new ListedBookService(supabaseClient);
});

builder.Services.AddScoped<GetBookIdDetailsService>(provider =>
{
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new GetBookIdDetailsService(supabaseClient);
});

builder.Services.AddScoped<SearchService>(provider =>
{
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new SearchService(supabaseClient);
});

builder.Services.AddScoped<RequstedTakedBookService>(provider =>
{
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new RequstedTakedBookService(supabaseClient);
});

builder.Services.AddScoped<ReturnedSavedBookService>(provider =>
{
    var supabaseClient = provider.GetRequiredService<Supabase.Client>();
    return new ReturnedSavedBookService(supabaseClient);
});

builder.Services.AddScoped<BookEventService>(provider =>
{
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new BookEventService(supabaseClient);
});


builder.Services.AddScoped<BookEventHistoryController>(provider =>
{
    var eventService = provider.GetService<BookEventService>();
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new BookEventHistoryController(eventService, supabaseClient);
});

builder.Services.AddScoped<BookEventHistoryController>(provider =>
{
    var eventService = provider.GetService<BookEventService>();
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new BookEventHistoryController(eventService, supabaseClient);
});

builder.Services.AddScoped<AddBookService>();
builder.Services.AddScoped<EditBookService>();
builder.Services.AddScoped<DeleteBookService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
    var issuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured");
    var audience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured");
    var tokenExpirationDays = builder.Configuration.GetValue<int>("Jwt:TokenExpirationDays", 7);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = "Токен истек" }));
            }
            else if (context.Exception.GetType() == typeof(SecurityTokenInvalidSignatureException))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = "Недействительная подпись токена" }));
            }
            else if (context.Exception.GetType() == typeof(SecurityTokenInvalidIssuerException))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = "Недействительный издатель токена" }));
            }
            else if (context.Exception.GetType() == typeof(SecurityTokenInvalidAudienceException))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = "Недействительная аудитория токена" }));
            }
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (accessToken.Count > 0 && path.StartsWithSegments("/api"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
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