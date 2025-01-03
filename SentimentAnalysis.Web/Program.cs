﻿using Autofac.Core;
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
builder.Services.AddSingleton<PeopleService>();
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<SentimentAnalysisContext>();
builder.Services.AddSingleton<IStatisticsService,StatisticsService>();

var Configuration = builder.Configuration;

// Configuración de autenticación
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

builder.Services.AddScoped<UserService>();

if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "Google";
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("Google", options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.Authority = "https://accounts.google.com";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.CallbackPath = "/signin-google";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("https://www.googleapis.com/auth/user.birthday.read"); // Para obtener fecha de nacimiento
        options.Scope.Add("https://www.googleapis.com/auth/user.gender.read"); // Perfil y género
    });
}

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

// Habilitar autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

// Configurar las rutas
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Blog}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "account",
        pattern: "{controller=Account}/{action=Login}/{returnUrl?}");
});

app.Run();