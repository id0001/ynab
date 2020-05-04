using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OsrsWeb.Vue.Controllers
{
	[ApiController]
	[Route("api/{controller}")]
	//[Authorize]
	public class YnabController : ControllerBase
	{
		public YnabController()
		{
		}

		[Route("categories")]
		public IActionResult GetCategories()
		{
			//if (!User.Identity.IsAuthenticated)
			//{
			//	return Challenge();
			//}

			return Ok(new string[] { "Aap", "Noot", "Mies" });
		}
	}
}
