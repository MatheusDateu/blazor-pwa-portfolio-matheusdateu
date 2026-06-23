using BlazorDeploy;
using BlazorDeploy.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using Supabase;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region Services

#region Scoped Services
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

builder.Services.AddScoped<ModalService>();
#endregion
#region Blazored
builder.Services.AddBlazoredLocalStorage();
#endregion
#region MudBlazor
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
#endregion
#region Location
builder.Services.AddLocalization(options => options.ResourcesPath = "");
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

var host = builder.Build();

#region Culture Setup
var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
var result = await localStorage.GetItemAsync<string>("culture");
CultureInfo culture;
if (!string.IsNullOrWhiteSpace(result))
{
    culture = new CultureInfo(result);
} else
{
    culture = new CultureInfo("pt-BR");
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
#endregion

await host.RunAsync();