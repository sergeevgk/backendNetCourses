using Lesson6Patterns.Decorator;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestProject
{
	public class CacheTests
	{
		[Fact]
		public void CacheDecoratorTest()
		{
			DateTime startDate = new DateTime(2010, 4, 8, 0, 0, 0);
			DateTime endDate = new DateTime(2020, 8, 4, 0, 0, 0);
			DateTime singleDate = new DateTime(2022, 2, 2, 2, 2, 2);

			var mockCurrencyRateLoader = new Mock<ICurrencyRateLoader>();
			mockCurrencyRateLoader
				.Setup(x => x.LoadSingleRate("USD", "RUB", singleDate))
				.Returns(new CurrencyValue
				{
					Currency = "USD",
					BaseCurrency = "RUB",
					TimeStamp = singleDate,
					Value = 75
				});

			mockCurrencyRateLoader
				.Setup(x => x.LoadRates("JPY", "KZT", startDate, endDate))
				.Returns(new[] {
					new CurrencyValue
					{
						Currency = "JPY",
						BaseCurrency = "KZT",
						TimeStamp = startDate,
						Value = 2
					},
					new CurrencyValue
					{
						Currency = "JPY",
						BaseCurrency = "KZT",
						TimeStamp = endDate,
						Value = 4
					}
				});

			var currencyRateLoader = mockCurrencyRateLoader.Object;

			var currencyRateLoaderDecorator = new CurrencyRateLoaderCacheDecorator(currencyRateLoader, 4);

			// loads with currencyRateLoader
			var singleValue = currencyRateLoaderDecorator.LoadSingleRate("USD", "RUB", singleDate);
			var arrayValues = currencyRateLoaderDecorator.LoadRates("JPY", "KZT", startDate, endDate);

			//loads from cache
			currencyRateLoaderDecorator.LoadSingleRate("USD", "RUB", singleDate);
			currencyRateLoaderDecorator.LoadRates("JPY", "KZT", startDate, endDate);

			// check for only one load (instead of two)
			mockCurrencyRateLoader.Verify(x => x.LoadSingleRate("USD", "RUB", singleDate), Times.Once);
			mockCurrencyRateLoader.Verify(x => x.LoadRates("JPY", "KZT", startDate, endDate), Times.Once);
		}

	}
}
