using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using CarDealership.Data;
using Microsoft.AspNetCore.Identity;
using CarDealership.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DealershipDatabase" ?? throw new InvalidOperationException("Invalid connection string"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSqlServer<CarsDbContext>(connectionString, opts => opts.EnableRetryOnFailure());

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<CarsDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<EmailService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Redirect if not logged in
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
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dealership}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseStaticFiles();

var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

// Create Admin role
if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}

// Create Admin user (Replace with your admin credentials)
var adminUser = await userManager.FindByEmailAsync("zenasagada.bincom@gmail.com");
if (adminUser == null)
{
    adminUser = new IdentityUser { UserName = "Zenas", Email = "zenasagada.bincom@gmail.com" };
    await userManager.CreateAsync(adminUser, "Password123!"); // Strong password
    await userManager.AddToRoleAsync(adminUser, "Admin");
}

app.Run();
