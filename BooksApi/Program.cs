using BooksApi.Controllers.BookEventController;
using BooksApi.Service.BookEventService;
using BooksApi.Service.BookService;
using BooksApi.Service.DashboardService;
using BooksApi.Service;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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
    return new UserAuthForm(supabaseClient);
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
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new ReturnedSavedBookService(supabaseClient);
});

builder.Services.AddScoped<BookEventService>(provider =>
{
    var supabaseClient = provider.GetService<Supabase.Client>();
    return new BookEventService(supabaseClient);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();