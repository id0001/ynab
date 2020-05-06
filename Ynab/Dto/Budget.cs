using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ynab.Dto
{
	public class Budget
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		public string Name { get; set; }
	}
}
