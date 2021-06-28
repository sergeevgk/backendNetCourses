using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson6Patterns.Decorator
{
	public class CurrencyValue
	{
		public string Currency { get; set; }
		public string BaseCurrency { get; set; }
		public decimal Value { get; set; }
		public DateTime TimeStamp { get; set; }
	}

}
