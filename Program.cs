using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using CarDealership.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DealershipDatabase" ?? throw new InvalidOperationException("Invalid connection string"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSqlServer<CarsDbContext>(connectionString, opts => opts.EnableRetryOnFailure());

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

app.Run();
