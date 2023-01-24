
using CloudinaryDotNet;
using Google.Apis.Books.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebook.Utilities;
using ReBook.FILTER;
using ReBook.Hubs;
using ReBook.Interfaces.Repository;
using ReBook.Interfaces.Services;
using ReBook.Models;
using ReBook.Repository;
using ReBook.Security;
using ReBook.Services;
using ReBook.Settings;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ReBookDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
    //options.Password.RequiredLength = 10;
    //options.Password.RequiredUniqueChars = 3;
    //options.Password.RequireNonAlphanumeric = false;
    //options.SignIn.RequireConfirmedEmail = true; 

    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
})
    .AddEntityFrameworkStores<ReBookDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

// Change la duree de vie de TOUS les types de Token (reset password... email confirmation...etc...)

builder.Services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(5));

// Pas très souple... Si on veut paramétrer des durées de vie différentes pour les différents types de token
// il faut créer des custom class qui étendent la classe "DataProtectionTokenProviderOptions"
// pour chaque type de token à personnaliser
// => voir CustomEmailConfirmationTokenProviderOptions dans le dossier "Security"
builder.Services.Configure<CustomEmailConfirmationTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(3));

var test = builder.Configuration.GetSection("MailSettings");


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddScoped<IEditionAPIService, GoogleBooksService>();
builder.Services.AddScoped<IEditionRepository, EditionRepository>();
builder.Services.AddScoped<ICopyRepository, CopyRepository>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<IWishRepository, WishRepository>();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
builder.Services.AddScoped<IJsonRenderService, JsonRenderService>();
builder.Services.AddScoped<AuthorisationFilter>();

//builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IEditionService, EditionService>();
builder.Services.AddScoped<IWishService, WishService>();
builder.Services.AddScoped<IAdministrationService, AdministrationService>();
builder.Services.AddScoped<ICopyService, CopyService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ITradeService, TradeService>();

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("OAuth:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("OAuth:ApiKey");
        options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
        //options.CallbackPath = "";
    });

builder.Services.Add(new ServiceDescriptor(typeof(Cloudinary),
c => new Cloudinary(new Account(
                                        builder.Configuration.GetValue<string>("AccountCloudinary:CloudName"),
                                        builder.Configuration.GetValue<string>("AccountCloudinary:ApiKey"),
                                        builder.Configuration.GetValue<string>("AccountCloudinary:ApiSecret"))),
                                    ServiceLifetime.Transient));

builder.Services.Add(new ServiceDescriptor(typeof(BooksService),
                                    c => new BooksService(
                                        new BaseClientService.Initializer()
                                        {
                                            ApiKey = builder.Configuration.GetValue<string>("BooksService:ApiKey"),
                                            ApplicationName = builder.Configuration.GetValue<string>("BooksService:ApplicationName")
                                        }),
                                    ServiceLifetime.Transient));

//https://codeburst.io/implement-recaptcha-in-asp-net-core-and-razor-pages-eed8ae720933
//builder.Services.AddReCaptcha(Configuration.GetSection("ReCaptcha"));


builder.Services.AddControllersWithViews();
//services.AddSignalR();
builder.Services.AddSignalR(e =>
{
    e.EnableDetailedErrors = true;
});
//services.AddSingleton<DataProtectionPurposeStrings>();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapHub<NotificationHub>("/signalServer");
});
app.Run();
