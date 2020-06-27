using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ynab.Authentication;

namespace Ynab.Controllers
{
    public class AuthController : ControllerBase
    {
        [Route("login")]
        [HttpGet]
        public async Task LoginAsync(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync(YnabDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = returnUrl, IsPersistent = true, AllowRefresh = true });
        }

        [Route("logout")]
        [HttpGet]
        [Authorize]
        public async Task LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var request = HttpContext.Request;
            string postLogoutUri = $"{request.Scheme}://{request.Host}";
            HttpContext.Response.Redirect(postLogoutUri);
        }

        [Route("user")]
        [HttpGet]
        [Authorize]
        public IActionResult GetUser()
        {
            var claims = User.Claims;

            return Ok(new
            {
                Id = claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimarySid).Value
            });
        }
    }
}
