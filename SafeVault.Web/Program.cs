/* SafeVault/src/SafeVault.Web/Program.cs */
using Microsoft.AspNetCore.Authentication.Cookies;
using SafeVault.Data;
using SafeVault.Web.Security;
using SafeVault.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connStr = builder.Configuration.GetConnectionString("SafeVaultDb")
             ?? "Server=localhost;Database=SafeVault;Trusted_Connection=True;TrustServerCertificate=True;";

builder.Services.AddSingleton<IUserRepository>(new SqlUserRepository(connStr));
builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddSingleton<AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";
        options.AccessDeniedPath = "/account/denied";
        options.Cookie.Name = "SafeVault.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
    ctx.Response.Headers["X-Frame-Options"] = "DENY";
    ctx.Response.Headers["Referrer-Policy"] = "no-referrer";
    ctx.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    ctx.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none';";
    await next();
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }
