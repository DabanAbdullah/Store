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
using Microsoft.AspNetCore.Authentication.Facebook;
using Store.DataAccess.DBInitializer;



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
builder.Services.AddIdentity<IdentityUser,IdentityRole>(options =>
{
    // Require the user's email to be confirmed before allowing login
  //  options.SignIn.RequireConfirmedEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Lockout duration after the specified number of failed access attempts
    options.Lockout.MaxFailedAccessAttempts = 5;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//add after identity
builder.Services.ConfigureApplicationCookie(option => {
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    option.LoginPath = $"/Identity/Account/Logout";
    option.LoginPath = $"/Identity/Account/Login";
});



//addsessions 
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromMinutes(100);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});


builder.Services.AddScoped<IDBInitializer, DBInitializer>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {

       options.ClientId = config.GetSection("Google")["ClientId"];
       options.ClientSecret = config.GetSection("Google")["ClientSecret"];
   })
   .AddFacebook(options =>
   {
       options.ClientId = config.GetSection("FB")["ClientId"];
       options.ClientSecret = config.GetSection("FB")["ClientSecret"];
       options.AccessDeniedPath = "/AccessDeniedPathInfo";
       
       

   })
   .AddMicrosoftAccount(microsoftOptions =>
   {
        microsoftOptions.ClientId = "15e40e4e-cfd8-438f-aa50-c8ec6837a1ed";
        microsoftOptions.ClientSecret = "144f1d59-2011-47e8-93c2-e50d6ee60a47";

     
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
app.UseSession();
SeedDatabase();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();


void SeedDatabase()
{
    using(var scope = app.Services.CreateScope())
    {
        var dbinit = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
        dbinit.Initialize();
    }
}