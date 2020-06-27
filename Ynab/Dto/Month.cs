using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ynab.Dto
{
	public class Month
	{
		[JsonPropertyName("month")]
		public DateTime Date { get; set; }

		public int Income { get; set; }

		public int Budgeted { get; set; }

		public bool Deleted { get; set; }

		public IDictionary<string, List<FlatCategory>> Categories { get; set; }
	}
}
