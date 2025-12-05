using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorDeploy;
using Supabase;
using BlazorDeploy.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region Services

#region Scoped Services
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<ModalService>();
#endregion

#endregion

#region Supabase Initialization
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:Key"];

var option = new SupabaseOptions
{
    AutoRefreshToken = true,
    AutoConnectRealtime = false,
};

builder.Services.AddSingleton(provider => new Supabase.Client(supabaseUrl!, supabaseKey!, option));
#endregion

await builder.Build().RunAsync();
