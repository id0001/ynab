using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ynab.Dto;

namespace Ynab
{
	public class YnabResponse<T>
	{
		public int StatusCode { get; set; }

		public int ServerKnowledge { get; set; }

		public T Value { get; set; }

		public Error Error { get; set; }

		public bool Success => StatusCode >= 200 && StatusCode < 300;
	}
}
