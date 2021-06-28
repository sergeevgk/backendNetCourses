using DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Database
{
	public class JsonDb : IDatabase
	{
		public IList<Factory> Factories { get; set; } = new List<Factory>();
		public IList<Unit> Units { get; set; } = new List<Unit>();
		public IList<Tank> Tanks { get; set; } = new List<Tank>();

		public JsonDb()
		{
		}

		public JsonDb(string fileName)
		{
			this.dbFileName = fileName;
		}

		public string dbFileName { get; set; }

		public async Task ReadAsync()
		{
			try
			{
				string json = await File.ReadAllTextAsync(dbFileName);
				JsonDTO dto = JsonSerializer.Deserialize<JsonDTO>(json);
				if (Factories.Count == 0 && Units.Count == 0 && Tanks.Count == 0)
				{
					Tanks = SeedTanks();
					Units = SeedUnits();
					Factories = SeedFactories();
					await SaveAsync();
					return;
				}
				Factories = dto.Factories;
				Units = dto.Units;
				Tanks = dto.Tanks;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error on reading database." + ex.Message);
			}
		}

		public async Task SaveAsync()
		{
			try
			{
				string json = JsonSerializer.Serialize(new JsonDTO {
					Factories = Factories,
					Units = Units,
					Tanks = Tanks 
				});
				await File.WriteAllTextAsync(dbFileName, json);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error on updating database." + ex.Message);
			}
		}

		/// <summary>
		/// initializes repository with factories, units and tanks according to tables in task
		/// </summary>
		public async Task SeedData()
		{
			if (File.Exists(dbFileName))
			{
				await ReadAsync();
			}
			else
			{
				Tanks = SeedTanks();
				Units = SeedUnits();
				Factories = SeedFactories();
				await SaveAsync();
			}
		}

		protected IList<Tank> SeedTanks()
		{
			return new Tank[]
			{
				new Tank(1, "Резервуар 1", 1500, 2000, 1),
				new Tank(2, "Резервуар 2", 2500, 3000, 1),
				new Tank(3, "Дополнительный резервуар 24", 3000, 3000, 2),
				new Tank(4, "Резервуар 35", 3000, 3000, 2),
				new Tank(5, "Резервуар 47", 4000, 5000, 2),
				new Tank(6, "Резервуар 256", 500, 500, 3),
			};
		}

		protected IList<Unit> SeedUnits()
		{
			Unit[] units = new Unit[]
			{
				new Unit(1, "ГФУ-1", 1),
				new Unit(2, "ГФУ-2", 1),
				new Unit(3, "АВТ-6", 2),
			};
			return units;
		}

		protected IList<Factory> SeedFactories()
		{
			Factory[] factories = new Factory[]
			{
				new Factory(1, "МНПЗ", "Московский нефтеперерабатывающий завод"),
				new Factory(2, "ОНПЗ", "Омский нефтеперерабатывающий завод")
			};
			return factories;
		}
	}
}
