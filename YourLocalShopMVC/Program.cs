using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourLocalShopMVC.Data;
using YourLocalShopMVC.Data.Accounts;
using YourLocalShopMVC.DataInventory;
using YourLocalShopMVC.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ShopInventoryContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AccountsDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddUserManager<UserManager<IdentityUser>>()
    .AddEntityFrameworkStores<AccountsDbContext>();

builder.Services.AddIdentityCore<CustomerAccount>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddSignInManager()
    .AddRoles<IdentityRole>()
    .AddUserManager<UserManager<CustomerAccount>>()
    .AddEntityFrameworkStores<AccountsDbContext>()
    .AddDefaultTokenProviders()
    ;

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("InventoryManagementRole",
        policy => policy.RequireRole("Staff"));
});

builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Items}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "shopRoute",
    pattern: "Items/AddToCart",
    defaults: new {controller = "Shop", action = "AddToCart"});

app.MapRazorPages();


app.Run();
