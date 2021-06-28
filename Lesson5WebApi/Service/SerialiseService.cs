using DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lesson5WebApi.Service
{
	public class SerialiseService
	{
		public static IEnumerable<UnitEvent> DeserializeEvents(string fileName)
		{
			var text = File.ReadAllText(fileName);
			var result = JsonSerializer.Deserialize<UnitEvent[]>(text);
			if (result == null || result.Length == 0)
			{
				return new List<UnitEvent>();
			}
			return result;
		}
	}
}
