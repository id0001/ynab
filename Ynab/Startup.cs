using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ynab.Authentication;
using Ynab.Extensions;
using Ynab.Services;

namespace Ynab
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = YnabDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
            })
            .AddYnab(options =>
            {
                options.ClientId = Configuration["ynab:clientid"];
                options.ClientSecret = Configuration["ynab:clientsecret"];
                options.Scope.Add("read-only");
                options.SaveTokens = true;

                options.Events.OnRedirectToAuthorizationEndpoint = RedirectToAuthorizationEndpointAsync;
                options.Events.OnCreatingTicket = async context =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();

                    using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                    {
                        var claims = new List<Claim>();

                        if (payload.RootElement.TryGetProperty(new[] { "data", "user", "id" }, out var identifier))
                        {
                            claims.Add(new Claim(ClaimTypes.PrimarySid, identifier.GetString()));
                        }

                        context.Identity.AddClaims(claims);
                    }
                };
            });

            services.AddAuthorization();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSpaStaticFiles(configureOptions => configureOptions.RootPath = "ClientApp/build");

            services.AddHttpClient<IYnabService, YnabService>(async (sp, client) =>
            {
                var contextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var token = await contextAccessor.HttpContext.GetTokenAsync("access_token");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(Configuration["ynab:apiendpoint"], UriKind.Absolute);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
            });
        }

        private static Task RedirectToAuthorizationEndpointAsync(RedirectContext<OAuthOptions> context)
        {
            bool isAjaxRequest = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjaxRequest || context.HttpContext.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.Headers["Location"] = context.RedirectUri;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }

            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        }
    }
}
