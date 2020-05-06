using System;
using System.Collections.Generic;

namespace Ynab.Dto
{
	public class CategoryGroup
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public bool Hidden { get; set; }

		public bool Deleted { get; set; }

		public ICollection<Category> Categories { get; set; }
	}
}
