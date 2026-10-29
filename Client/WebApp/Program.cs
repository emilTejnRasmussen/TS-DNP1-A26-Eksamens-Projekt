using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Authentication;
using WebApp.Components;
using WebApp.Services.HttpImpl;
using WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "yap.auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = false;
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// API client for login and registration HTTP endpoints.
builder.Services.AddHttpClient("api", client => { client.BaseAddress = new Uri("http://localhost:5092"); });

// API client for interactive Blazor components.
builder.Services.AddScoped(sp =>
{
    var authProvider =
        sp.GetRequiredService<AuthenticationStateProvider>();

    var handler = new AuthTokenHandler(authProvider)
    {
        InnerHandler = new HttpClientHandler()
    };

    return new HttpClient(handler, disposeHandler: true)
    {
        BaseAddress = new Uri("http://localhost:5092")
    };
});

builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<IPostService, HttpPostService>();
builder.Services.AddScoped<ICommentService, HttpCommentService>();
builder.Services.AddScoped<ISubforumService, HttpSubforumService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapSessionEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();