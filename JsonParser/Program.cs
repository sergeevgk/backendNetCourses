using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace JsonParser
{
	class Program
	{
		static void Main(string[] args)
		{
			JsonParser parser = new JsonParser();
			string workingDirectory = Environment.CurrentDirectory;
			var items = parser.ParseDocument(@"../../../JSON_sample_1.json");
			foreach (var i in items)
			{
				Console.WriteLine(i.Date + " " + i.Id);
			}
			foreach (var id in parser.GetNumbersOfDeals(items))
			{
				Console.WriteLine(id);
			}
		}
	}

	public interface IJsonParser
	{
		public Deal[] ParseDocument(string fileName);
		IList<int> GetNumbersOfDeals(IEnumerable<Deal> deals);
	}

	public class JsonParser : IJsonParser
	{
		private readonly string idPrefix = "ABC";
		public IList<int> GetNumbersOfDeals(IEnumerable<Deal> deals)
		{
			return deals.Where(d => d.Sum >= 100)
				.OrderBy(d => d.Date).ThenByDescending(d => d.Sum)
				.Take(3)
				.Select(d => Int32.Parse(d.Id.Substring(idPrefix.Length)))
				.ToList();
		}

		public Deal[] ParseDocument(string fileName)
		{
			using (StreamReader r = new StreamReader(fileName))
			{
				string json = r.ReadToEnd();
				Deal[] items = JsonSerializer.Deserialize<Deal[]>(json);
				return items;
			}
		}
	}
	public class Deal
	{
		public DateTime Date { get; set; }
		public string Id { get; set; }
		public int Sum { get; set; }
	}

}
