using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TGM.Models;
using TGM.Models.DataBase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TgmDBContext>(
    options => options.UseSqlite(GetConnString())
);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
builder.Services.AddAuthorization();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

static string GetConnString()
{
    var folder = Environment.SpecialFolder.Personal;
    var path = Environment.GetFolderPath(folder);
    var dbPath = Path.Join(path, "TractirGameMaster.db");
    var connectionString = $"Data source={dbPath}";
    return connectionString;
}
