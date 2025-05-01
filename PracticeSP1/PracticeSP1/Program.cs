using Microsoft.EntityFrameworkCore;
using PracticeSP1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>
    (op => op.UseSqlServer(builder.Configuration.GetConnectionString("con")));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );



//app.MapGet("/", () => "Hello World!");

app.Run();
