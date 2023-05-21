using BlazorCRUDApp.Server.AppDbContext;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//var logger = new LoggerConfiguration()
//  .ReadFrom.Configuration(builder.Configuration)
//  .Enrich.FromLogContext()
//  .CreateLogger();
//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(logger);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;

});

//azure
//builder.Services.Configure<IISServerOptions>(options =>
//{
//    options.AllowSynchronousIO = true;
//});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseWebAssemblyDebugging();
    app.UseExceptionHandler("/Error");
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