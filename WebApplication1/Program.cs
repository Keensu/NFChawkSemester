using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NFChawk.Background;
using NFChawk.Data;
using NFChawk.Hubs;
using NFChawk.Models.Entities;
using NFChawk.Models.Interfaces;
using NFChawk.Models.ViewModels;
using NFChawk.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseNpgsql(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information)
);

//Identity
builder.Services.AddIdentity<User, Role>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


//Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    
    options.LoginPath = "/Auth/LogIn";
    options.LogoutPath = "/Auth/LogOut";
    options.AccessDeniedPath = "/Error/Error403";
    options.Cookie.Name = "NFChawk.Auth";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

});

//SignalR
builder.Services.AddSignalR();

//Email service settings
builder.Services.Configure<EmailServiceViewModel>(builder.Configuration
    .GetSection("EmailSettings"));

builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var model = config.GetSection("EmailSettings").Get<EmailServiceViewModel>();
    if (model == null)
    {
        throw new InvalidOperationException("EmailSettings section is missing or invalid in configuration.");
    }
    return model;
});


//Custom services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INFTService, NFTService>();
builder.Services.AddScoped<ICollectionService, CollectionService>();
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAuctionFinalizationService, AuctionFinalizationService>();
builder.Services.AddHostedService<AuctionBackgroundService>();


//File upload settings
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600;
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {

        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        await DataInitializer.SeedData(userManager, roleManager, configuration);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication(); 
app.UseAuthorization();  

app.MapStaticAssets();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<AuctionHub>("/auctionHub");

app.Run();