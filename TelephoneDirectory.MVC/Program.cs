using System.Security.Claims;
using System.Text;
using System.Web.Mvc;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using TelephoneDirectory.Business.DependencyResolvers.Autofac;
using Microsoft.AspNetCore.Diagnostics;
using TelephoneDirectory.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.Entities.Concrete;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TelephoneDirectory.Core.DependencyResolvers;
using TelephoneDirectory.Core.Utilities.IoC;
using TelephoneDirectory.Core.Utilities.Security.Encryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Autofac.Core;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Authentication.Cookies;
using TelephoneDirectory.Core.Utilities.Security.JWT;
using Microsoft.EntityFrameworkCore;
using TelephoneDirectory.Business.BusinessAspects.Autofac;
using TelephoneDirectory.DataAccess.Concrete;
using Microsoft.AspNetCore.Mvc.Filters;
using TelephoneDirectory.Business.ValidationRules.FluentValidation;
using TelephoneDirectory.Core.Aspects.AutofacAspect;
using TelephoneDirectory.MVC.Filters;
using FluentValidation;
using TelephoneDirectory.Business.Concrete;


var builder = WebApplication.CreateBuilder(args);


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory
    ()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Login";
        options.DefaultSignInScheme = "Login";
    }).AddCookie("Login", options =>
        {
            options.LoginPath = "/Users/Login";
            //options.AccessDeniedPath = "/Users/AccessDenied";
            options.LogoutPath = "/Users/Logout";
            options.AccessDeniedPath = "/Error/PageNotFound";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.Cookie.Name = "LoginCookie";

        }).AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["TokenOptions:Issuer"],
                ValidAudience = builder.Configuration["TokenOptions:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenOptions:SecurityKey"]))
            };
        });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Moderator", policy =>
        policy.RequireClaim("Role", "Moderator"));
});
builder.Services.AddDependencyResolvers(new ICoreModule[] { new CoreModule() });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/PageNotFound");
app.ConfigureCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Persons}/{action=Index}/{id?}");

app.Run();