using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace Ynab.Authentication
{
	public class YnabOptions : OAuthOptions
	{
		public YnabOptions()
		{
			CallbackPath = new PathString("/signin-ynab");
			AuthorizationEndpoint = YnabDefaults.AuthorizationEndpoint;
			TokenEndpoint = YnabDefaults.TokenEndpoint;
			UserInformationEndpoint = YnabDefaults.UserInformationEndpoint;
		}
	}
}
