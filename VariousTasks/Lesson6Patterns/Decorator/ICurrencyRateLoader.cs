using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson6Patterns.Decorator
{
	public interface ICurrencyRateLoader
	{
		CurrencyValue LoadSingleRate(string currency, string baseCurrency, DateTime date);
		IReadOnlyCollection<CurrencyValue> LoadRates(string currency, string baseCurrency, DateTime startDate, DateTime endDate);
	}

}
