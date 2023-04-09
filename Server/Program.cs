using BlazorCRUDApp.Server.AppDbContext;
using BlazorCRUDApp.Server.Models;
using BlazorCRUDApp.Server.Repository;
using BlazorCRUDApp.Server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//var logger = new LoggerConfiguration().wri
//        .ReadFrom.Configuration(builder.Configuration)
//        .Enrich.FromLogContext()
//        .CreateLogger();
//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(logger);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;

});

//azure
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseWebAssemblyDebugging();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

app.UseEndpoints(endpoints => endpoints.MapFallbackToFile("/webui/{*path:nonfile}", "webui/index.html"));
app.UseBlazorFrameworkFiles(new PathString("/webui"));
app.UseStaticFiles("/webui");
app.Run();
//app.Run();
