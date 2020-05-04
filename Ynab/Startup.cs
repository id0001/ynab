using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ynab
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = "Ynab";
			})
			.AddCookie()
			.AddOAuth("Ynab", options =>
			{
				options.ClientId = "84f6a7bc0f601375c0c6dfabf8e1c48c17e3515654528403fbc733122cda981b";
				options.ClientSecret = "99da34a09dc855c24f003e04e76298ae8b4b149b2baed8db2c00f6edfe02023b";
				options.CallbackPath = new PathString("/signin-ynab");

				options.AuthorizationEndpoint = "https://app.youneedabudget.com/oauth/authorize";
				options.TokenEndpoint = "https://app.youneedabudget.com/oauth/token";
			});

			services.AddAuthorization(options =>
			{
			});

			services.AddSpaStaticFiles(configureOptions => configureOptions.RootPath = "ClientApp/build");
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
	}
}
