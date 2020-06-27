using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ynab.Dto
{
	public class Category
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public bool Hidden { get; set; }

		public bool Deleted { get; set; }

		public int Budgeted { get; set; }
	}
}
