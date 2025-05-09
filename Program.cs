using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using ManyRoomStudio.Autofac;
using ManyRoomStudio.Infrastructure.Filters;
using ManyRoomStudio.Infrastructure.Helpers;
using ManyRoomStudio.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
        optional: true, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddCors(c =>
//{
//    c.AddPolicy("AllowOrigin",
//      policy =>
//      {
//          policy
//            .WithOrigins("http://localhost:3000", "https://localhost:7178", "https://localhost:5283/")
//            .AllowAnyOrigin()
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//      });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod() // includes PUT
            .AllowAnyHeader();
    });
});

//var config = new AppConfiguration();
//builder.Configuration.Bind("DefaultConnection", connectionString);
//builder.Configuration.Bind("MailSetting", config);
//builder.Services.AddSingleton(config);

// Add Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register Autofac modules
builder.Services.AddAutofac();

builder.Services.AddDbContext<ManyRoomStudioContext>(options =>
    options.UseSqlServer(connectionString!));

builder.Services.AddCascadingAuthenticationState();


builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Gatway
//builder.Services.AddScoped<IUserGatway , UserGatway>();
//builder.Services.AddScoped<IRoleGatway, RoleGatway>();
//builder.Services.AddScoped<IRoomGateway, RoomGateway>();
//builder.Services.AddScoped<IRoomImageGateway, RoomImageGateway>();
//builder.Services.AddScoped<IMasterDetailsGateway, MasterDetailsGateway>();
//builder.Services.AddScoped<IRoomEventGateway, RoomEventGateway>();
//builder.Services.AddScoped<IBookingGateway, BookingGateway>();
//builder.Services.AddScoped<IRoomStaffMappingGateway, RoomStaffMappingGateway>();
//builder.Services.AddScoped<IFranchiseekeyGateway, FranchiseekeyGateway>();
//builder.Services.AddScoped<IRoomFranchiseeAdminMappingGateway, RoomFranchiseeAdminMappingGateway>();
//builder.Services.AddScoped<IErrorGateway, ErrorGateway>();


//vw model 
//builder.Services.AddScoped<IvwStaffModelGateway, vwStaffModelGateway>();


//builder.Services.AddScoped<IRazorPartialToString, RazorPartialToString>();

//UseCase
//builder.Services.AddScoped<IFileuploadUsecase, FileuploadUsecase>();
//builder.Services.AddScoped<IRoomCreateUsecase, RoomCreateUsecase>();
//builder.Services.AddScoped<IRoomSearchUsecase, RoomSearchUsecase>();
//builder.Services.AddScoped<IStudioBookUsecase, StudioBookUsecase>();
//builder.Services.AddScoped<IUserBookingUsecase, UserBookingUsecase>();
//builder.Services.AddScoped<IRoomManagementUsecase, RoomManagementUsecase>();
//builder.Services.AddScoped<IFranchiseeByIdUsecase, FranchiseeByIdUsecase>();
//builder.Services.AddScoped<IFranchiseeRoomMappingByUserIdUsecase, FranchiseeRoomMappingByUserIdUsecase>();
//builder.Services.AddScoped<IAdminStaffUsecase, AdminStaffUsecase>();
//builder.Services.AddScoped<IStaffRoomMappingByUserIdUsecase, StaffRoomMappingByUserIdUsecase>();

builder.Services.AddControllers();

builder.Services.AddControllersWithViews(o =>
{
    o.Filters.Add<ApiExceptionFilter>();
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<AutofacConfig>();  
});

//builder.Services.AddControllersWithViews(o =>
//{
//    o.Filters.Add<ApiExceptionFilter>();
//}).AddRazorOptions(x =>
//{
//    x.ViewLocationFormats.Clear();
//    x.ViewLocationFormats.Add("/Views/Login/{0}.cshtml");
//    x.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
//}).AddRazorRuntimeCompilation();

// Add services to the container.
//builder.Services.AddControllersWithViews();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
           opt.TokenLifespan = TimeSpan.FromHours(8));

builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
opt.TokenLifespan = TimeSpan.FromHours(8));

builder.Services.ConfigureApplicationCookie(opt => opt.LoginPath = "/Login/Index");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});



builder.Services.AddAuthorization();

var jwtstring = builder.Configuration.GetSection("JwtSettings:SecretKey").Value;
var key = Encoding.ASCII.GetBytes(jwtstring);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(x =>
   {
       x.RequireHttpsMetadata = false;
       x.SaveToken = true;
       x.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(key),
           ValidateIssuer = false,
           ValidateAudience = false
       };
   });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Many Rooms Studio API",
        Version = "v1",
        Description = "A simple example ASP.NET Core Web API"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();