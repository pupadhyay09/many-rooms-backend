using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Register session and caching
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Persist Data Protection keys in Docker volume or host folder
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/keys")) // Make sure volume is mounted
    .SetApplicationName("ManyRoomStudio");

// Configure Autofac if needed
builder.Host.UseServiceProviderFactory(new Autofac.Extensions.DependencyInjection.AutofacServiceProviderFactory());

var app = builder.Build();

// Use forwarded headers when running behind proxy (e.g., Nginx or Docker networks)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Serve static files from wwwroot
app.UseStaticFiles();

app.UseRouting();

// Enable session
app.UseSession();

// Use authentication/authorization if needed
app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
