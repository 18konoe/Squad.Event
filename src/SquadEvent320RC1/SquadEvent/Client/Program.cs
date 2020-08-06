using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SquadEvent.Client.Services;
using Syncfusion.Blazor;

namespace SquadEvent.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Register Syncfusion license 
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQ5NTExQDMxMzgyZTMxMmUzMERaYWlrQ0tmSUZlU2dFbExmbEJCN1pINmNUemdBOVR6Ujh1ejQ1RnNJSjQ9");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Additional
            builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSyncfusionBlazor();

            await builder.Build().RunAsync();
        }
    }
}
