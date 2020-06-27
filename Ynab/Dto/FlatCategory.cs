using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ynab.Dto
{
	public class FlatCategory
	{
		public string Id { get; set; }

		public int Budgeted { get; set; }
	}
}
