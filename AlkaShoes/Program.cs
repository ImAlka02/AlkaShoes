using AlkaShoes.Models.Entities;
using AlkaShoes.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddDbContext<AlkashoesContext>(x=>x.UseMySql("user=root;password=root;server=localhost;database=alkashoes", 
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));

builder.Services.AddTransient<Repo<Marca>>();
builder.Services.AddTransient<Repo<User>>();
builder.Services.AddTransient<RepoCarrito>();
builder.Services.AddTransient<RepoProductos>();
builder.Services.AddTransient<RepoTallas>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.AccessDeniedPath = "/Home/Denied";
    x.LoginPath = "/Home/Login";
    x.LogoutPath = "/Home/Logout";
    x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    x.Cookie.Name = "alkashoesCookie";
});

var app = builder.Build();
app.UseFileServer();
app.MapControllerRoute(
            name : "areas",
            pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapDefaultControllerRoute();

app.Run();
