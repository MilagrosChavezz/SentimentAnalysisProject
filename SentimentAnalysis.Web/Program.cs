using Autofac.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SentimentAnalysis.Data.Entities;
using SentimentAnalysis.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<MLAnalysis>();

var Configuration = builder.Configuration;

// Configurar DbContext
builder.Services.AddDbContext<SentimentAnalysisContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("SentimentAnalysisContext")));

// Configuración de autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Google";
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) // Almacena la información de sesión en cookies
.AddOpenIdConnect("Google", options =>
{
   
    options.Authority = "https://accounts.google.com"; // URL del proveedor de Google
    options.ResponseType = "code";
    options.SaveTokens = true; // Guarda los tokens recibidos
    options.CallbackPath = "/signin-google"; // Ruta para manejar la respuesta de Google

    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configurar las rutas
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Prediction}/{action=Predict}/{id?}");

    endpoints.MapControllerRoute(
        name: "account",
        pattern: "{controller=Account}/{action=Login}/{returnUrl?}");
});

app.Run();
