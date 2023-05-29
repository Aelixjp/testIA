//FIREBASE
// main namespaces
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using TestIA_Blazor.Areas.Identity;
using TestIA_Blazor.Data;
using TestIA_Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<IAnswerGeneratorService, AnswerGeneratorService>();

const string FIREBASE_API_KEY = "AIzaSyA7raLW_TN3tjK7hr2kghTGs0bwg6x8DyY";
const string FIREBASE_AUTH_DOMAIN = "test-1e7ea.firebaseapp.com";


// Configure...
var config = new FirebaseAuthConfig
{
    ApiKey = FIREBASE_API_KEY,
    AuthDomain = FIREBASE_AUTH_DOMAIN,
    Providers = new FirebaseAuthProvider[]
    {
        new EmailProvider()
    }
};

// ...and create your FirebaseAuthClient
var client = new FirebaseAuthClient(config);


var userLooged = await client.SignInWithEmailAndPasswordAsync("ejemplo@hotmail.com", "Ejemplo#123");


Console.WriteLine(userLooged.User.Uid);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
