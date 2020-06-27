using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
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
		private const string SpaRoot = "app";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		private IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			AddAuthentication(services);
			services.AddAuthorization();

			services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSpaStaticFiles(configureOptions => configureOptions.RootPath = $"{SpaRoot}/build");

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
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSpaStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(options =>
			{
				options.MapControllers();
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = SpaRoot;

				if (env.IsDevelopment())
					spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
			});
		}

		private void AddAuthentication(IServiceCollection services)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = YnabDefaults.AuthenticationScheme;
			})
			.AddCookie(options =>
			{
				options.Events.OnValidatePrincipal = OnValidatePrincipalAsync;
			})
			.AddYnab(options =>
			{
				options.ClientId = Configuration["ynab:clientid"];
				options.ClientSecret = Configuration["ynab:clientsecret"];
				options.Scope.Add("read-only");
				options.SaveTokens = true;

				options.Events.OnRedirectToAuthorizationEndpoint = OnRedirectToAuthorizationEndpointAsync;
				options.Events.OnCreatingTicket = OnCreatingTicketAsync;
			});
		}

		private static async Task OnValidatePrincipalAsync(CookieValidatePrincipalContext context)
		{
			const string accessTokenName = "access_token";
			const string refreshTokenName = "refresh_token";
			const string expirationTokenName = "expires_at";
			const string expiresInName = "expires_in";

			if (context.Principal.Identity.IsAuthenticated)
			{
				var expires = context.Properties.GetTokenValue(expirationTokenName);
				if (expires != null && DateTime.Parse(expires, CultureInfo.InvariantCulture).ToUniversalTime() < DateTime.UtcNow)
				{
					var refreshToken = context.Properties.GetTokenValue(refreshTokenName);
					if (refreshToken == null)
					{
						context.RejectPrincipal();
						return;
					}

					var serviceProvider = context.HttpContext.RequestServices;
					var ynabOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<YnabOptions>>().Get(YnabDefaults.AuthenticationScheme);

					var pairs = new Dictionary<string, string>
							{
								{ "client_id", ynabOptions.ClientId },
								{ "client_secret", ynabOptions.ClientSecret },
								{ "grant_type", "refresh_token"},
								{ "refresh_token", refreshToken }
							};

					var content = new FormUrlEncodedContent(pairs);
					var refreshResponse = await ynabOptions.Backchannel.PostAsync(ynabOptions.TokenEndpoint, content, context.HttpContext.RequestAborted);
					refreshResponse.EnsureSuccessStatusCode();

					using var payload = JsonDocument.Parse(await refreshResponse.Content.ReadAsStringAsync());

					var newAccessToken = payload.RootElement.GetProperty(accessTokenName).GetString();
					var newRefreshToken = payload.RootElement.GetProperty(refreshTokenName).GetString();
					var newExpirationToken = DateTime.UtcNow.AddSeconds(payload.RootElement.GetProperty(expiresInName).GetInt32()).ToString("o", CultureInfo.InvariantCulture);

					context.Properties.StoreTokens(new[]
					{
								new AuthenticationToken{ Name = refreshTokenName, Value = newRefreshToken },
								new AuthenticationToken{ Name = accessTokenName, Value = newAccessToken },
								new AuthenticationToken{ Name = expirationTokenName, Value= newExpirationToken}
							});

					context.ShouldRenew = true;
				}
			}
		}

		private static Task OnRedirectToAuthorizationEndpointAsync(RedirectContext<OAuthOptions> context)
		{
			if(!context.HttpContext.Request.Path.StartsWithSegments("/login"))
			{
				context.Response.Headers["Location"] = context.RedirectUri;
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				return Task.CompletedTask;
			}

			context.Response.Redirect(context.RedirectUri);
			return Task.CompletedTask;
		}

		private static async Task OnCreatingTicketAsync(OAuthCreatingTicketContext context)
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
		}
	}
}
