using AlkaShoes.Models.Entities;
using AlkaShoes.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddDbContext<AlkashoesContext>(x=>x.UseMySql("user=websitos_AlkaShoes;password=3gy71j8?G;server=websitos256.com;database=websitos_AlkaShoes", 
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));

builder.Services.AddTransient<Repo<Marca>>();
builder.Services.AddTransient<Repo<User>>();
builder.Services.AddTransient<RepoCarrito>();
builder.Services.AddTransient<RepoProductos>();
builder.Services.AddTransient<RepoTallas>();
builder.Services.AddTransient<Repo<Talla>>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.AccessDeniedPath = "/Home/Denied";
    x.LoginPath = "/Home/Index";
    x.LogoutPath = "/Home/Logout";
    x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    x.Cookie.Name = "alkashoesCookie";
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFileServer();
app.MapControllerRoute(
            name : "areas",
            pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapDefaultControllerRoute();

app.Run();
