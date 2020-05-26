using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ynab.Authentication
{
	public static class YnabExtensions
	{
		public static AuthenticationBuilder AddYnab(this AuthenticationBuilder builder)
			=> builder.AddYnab(YnabDefaults.AuthenticationScheme, _ => { });

		public static AuthenticationBuilder AddYnab(this AuthenticationBuilder builder, Action<YnabOptions> configureOptions)
			=> builder.AddYnab(YnabDefaults.AuthenticationScheme, configureOptions);

		public static AuthenticationBuilder AddYnab(this AuthenticationBuilder builder, string authenticationScheme, Action<YnabOptions> configureOptions)
			=> AddYnab(builder, authenticationScheme, YnabDefaults.DisplayName, configureOptions);

		public static AuthenticationBuilder AddYnab(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<YnabOptions> configureOptions)
		{
			return builder.AddOAuth<YnabOptions, OAuthHandler<YnabOptions>>(authenticationScheme, displayName, configureOptions);
		}
	}
}
