using TicketingSystem.BlazorUI.Components;
using TicketingSystem.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DIP: Injecting HttpClient to interact with the API layer
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5030/") }); // Adjust port as needed

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
