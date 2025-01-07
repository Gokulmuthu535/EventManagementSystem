using EventManagementSystem.Repositories;
using EventManagementSystem.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Access the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DBConnection");

// Register the repository and interface for dependency injection
builder.Services.AddScoped<IEventRepository, EventRepository>(provider =>
    new EventRepository(connectionString)); // Pass the connection string to the repository
builder.Services.AddScoped<IUserRepository, UserRepository>(provider =>
    new UserRepository(connectionString));
builder.Services.AddScoped<IBookingRepository, BookingRepository>(provider =>
    new BookingRepository(connectionString));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession(); //  to enable session middleware


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
