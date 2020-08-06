using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SquadEvent.Server.Model;
using SquadEvent.Server.Services;
using SquadEvent.Shared.Models;

namespace SquadEvent.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, DummyAuthenticationStateProvider>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                    
                    // https://stackoverflow.com/a/44600389/1268000
                    options.Events.OnRedirectToAccessDenied = async (ctx) =>
                    {
                        ctx.Response.StatusCode = 403;
                        var message = Encoding.UTF8.GetBytes("Access Denied.");
                        await ctx.Response.Body.WriteAsync(message, 0, message.Length);
                    };
                })
                .AddDiscord(option =>
                {
                    option.ClientId = "685048791295983634";
                    option.ClientSecret = "sZTTUEEpZD-a1jqr-24HUqTnwB-ZoK12";
                    option.Scope.Add("identify");
                    option.Scope.Add("email");
                    option.Scope.Add("connections");
                    option.Scope.Add("guilds");
                    option.SaveTokens = true;
                    option.ClaimActions.MapJsonKey("avatar", "avatar");

                    option.Events.OnCreatingTicket = context =>
                    {
                        var accessToken = context.AccessToken;
                        var refreshToken = context.RefreshToken;
                        var tokenType = context.TokenType;
                        var expiresIn = context.ExpiresIn;
                        context.Identity.AddClaim(new Claim(nameof(context.AccessToken), context.AccessToken));
                        context.Identity.AddClaim(new Claim(nameof(context.RefreshToken), context.RefreshToken));
                        context.Identity.AddClaim(new Claim(nameof(context.TokenType), context.TokenType));
                        context.Identity.AddClaim(new Claim(nameof(context.ExpiresIn), context.ExpiresIn.ToString()));

                        return Task.CompletedTask;
                    };

                    option.Events.OnRemoteFailure = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                });
            services.AddControllersWithViews();

            var test = Configuration.GetSection(nameof(EventsStoreDatabaseSettings));
            
            services.Configure<EventsStoreDatabaseSettings>(Configuration.GetSection(nameof(EventsStoreDatabaseSettings)));
            services.AddSingleton<IEventsStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<EventsStoreDatabaseSettings>>().Value);
            services.AddSingleton<IMongoCollectionProvider<EventModel>, EventCollectionProvider>();
            services.AddSingleton<IEventService, EventService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
