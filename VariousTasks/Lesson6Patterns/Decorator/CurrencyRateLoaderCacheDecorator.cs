using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Lesson6Patterns.Decorator
{
	public class CurrencyRateLoaderCacheDecorator : ICurrencyRateLoader
	{
		private readonly OrderedDictionary currencyRatesCache = new OrderedDictionary();
		private readonly ICurrencyRateLoader currencyRateLoader;
		private readonly int cacheMaxVolume;
		private int currentVolume = 0;

		public CurrencyRateLoaderCacheDecorator(ICurrencyRateLoader currencyRateLoader, int cacheVolume)
		{
			this.currencyRateLoader = currencyRateLoader;
			this.cacheMaxVolume = cacheVolume;
		}

		public IReadOnlyCollection<CurrencyValue> LoadRates(string currency, string baseCurrency, DateTime startDate, DateTime endDate)
		{
			var cachedKey = GetKey(currency, baseCurrency, startDate, endDate);
			if (currencyRatesCache.Contains(cachedKey))
			{
				return (IReadOnlyCollection<CurrencyValue>)currencyRatesCache[cachedKey];
			}
			var newRateCollection = currencyRateLoader.LoadRates(currency, baseCurrency, startDate, endDate);
			CacheValues(currencyRatesCache, cachedKey, newRateCollection);
			return newRateCollection;
		}
		public CurrencyValue LoadSingleRate(string currency, string baseCurrency, DateTime date)
		{
			var cachedKey = GetKey(currency, baseCurrency, date);
			if (currencyRatesCache.Contains(cachedKey))
			{
				return (CurrencyValue)currencyRatesCache[cachedKey];
			}
			var newRate = currencyRateLoader.LoadSingleRate(currency, baseCurrency, date);
			CacheValues(currencyRatesCache, cachedKey, newRate);
			return newRate;
		}

		private void CacheValues(OrderedDictionary dict, object key, object value)
		{
			switch (value)
			{
				case CurrencyValue:
					if (currentVolume + 1 > cacheMaxVolume)
					{
						DeleteLastCachedValues(dict);
					}
					currentVolume++;
					dict.Add(key, value);
					break;
				case IReadOnlyCollection<CurrencyValue> currencyValues:
					if (currencyValues.Count > cacheMaxVolume)
					{
						throw new ArgumentOutOfRangeException("Attempt to cache too large object.");
					}
					while (currentVolume + currencyValues.Count > cacheMaxVolume)
					{
						DeleteLastCachedValues(dict);
					}
					currentVolume += currencyValues.Count;
					dict.Add(key, value);
					break;
				default:
					break;
			}
		}

		private void DeleteLastCachedValues(OrderedDictionary dict)
		{
			if (dict.Count == 0)
			{
				throw new ArgumentOutOfRangeException("Attempt to delete object from empty cache.");
			}
			var value = dict[0];
			switch (value)
			{
				case CurrencyValue:
					currentVolume--;
					break;
				case IReadOnlyCollection<CurrencyValue> currencyValues:
					currentVolume -= currencyValues.Count;
					break;
				default:
					break;
			}
			dict.RemoveAt(0);
		}

		private string GetKey(string currency, string baseCurrency, DateTime date1, DateTime? date2 = null)
		{
			var date2String = date2?.ToString() ?? "";
			return currency + baseCurrency + date1.ToString() + date2String;
		}

	}
}
