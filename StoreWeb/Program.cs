using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Data;
using Store.DataAccess.Repository.IRepository;
using Store.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Store.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Store.Models;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;


builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//register database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnectionString")));

builder.Services.AddScoped<IEmailSender,EmailSender>();
//builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//add after identity
builder.Services.ConfigureApplicationCookie(option => {
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    option.LoginPath = $"/Identity/Account/Logout";
    option.LoginPath = $"/Identity/Account/Login";
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       IConfigurationSection googleAuthNSection =
       config.GetSection("Authentication:Google");
       // options.ClientId = googleAuthNSection["ClientId"];
       // options.ClientSecret = googleAuthNSection["ClientSecret"];
       options.ClientId = "sdfsfdsfs";
       options.ClientSecret = "sdfsfdsfs";
   })
   .AddFacebook(options =>
   {
       IConfigurationSection FBAuthNSection =
       config.GetSection("Authentication:FB");
       //  options.ClientId = FBAuthNSection["ClientId"];
       //  options.ClientSecret = FBAuthNSection["ClientSecret"];
       options.ClientId = "sdfsfdsfs";
       options.ClientSecret = "sdfsfdsfs";
   })
   .AddMicrosoftAccount(microsoftOptions =>
   {
       //  microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
       // microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];

       microsoftOptions.ClientId = "sdfsfdsfs";
       microsoftOptions.ClientSecret = "sdfsfdsfs";
   })
   .AddTwitter(twitterOptions =>
   {
       //  twitterOptions.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
       // twitterOptions.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
       twitterOptions.ConsumerKey = "sdfsfdsfs";
       twitterOptions.ConsumerSecret = "sdfsfdsfs";
      
       twitterOptions.RetrieveUserDetails = true;
   });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:secretkey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
