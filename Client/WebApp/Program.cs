using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Authentication;
using WebApp.Components;
using WebApp.Services.HttpImpl;
using WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Blazor authentication
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<TokenStore>();

builder.Services.AddScoped<JwtAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(sp => 
    sp.GetRequiredService<JwtAuthenticationStateProvider>());


builder.Services.AddScoped(sp =>
{
    var tokenStore = sp.GetRequiredService<TokenStore>();

    var handler = new AuthTokenHandler(tokenStore)
    {
        InnerHandler = new HttpClientHandler()
    };

    return new HttpClient(handler, disposeHandler: true)
    {
        BaseAddress = new Uri("http://localhost:5092")
    };
});

// Authentication service
builder.Services.AddScoped<IAuthService, HttpAuthService>();

// Existing HTTP services
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
    createScopeForStatusCodePages: true
);

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();