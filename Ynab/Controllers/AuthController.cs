using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ynab.Authentication;

namespace Ynab.Controllers
{
	public class AuthController : ControllerBase
	{
		[Route("login")]
		public async Task LoginAsync(string returnUrl = "/")
		{
			await HttpContext.ChallengeAsync(YnabDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = returnUrl });
		}

		[Route("logout")]
		[Authorize]
		public async Task Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			var request = HttpContext.Request;
			string postLogoutUri = $"{request.Scheme}://{request.Host}";
			HttpContext.Response.Redirect(postLogoutUri);
		}
	}
}
