using System.Collections.Generic;

namespace DAL.Models
{
	public class Unit : NamedDbObject
	{
		public Unit() : base()
		{
		}

		public Unit(int id, string name, int factoryId) : base(id, name)
		{
			FactoryId = factoryId;
		}
		public int FactoryId { get; set; }
		public Factory Factory;
		public IList<Tank> Tanks { get; set; }
		public override string ToString()
		{
			return $"[Id: {Id}, Name: {Name}, FactoryId: {FactoryId}]";
		}
	}
}
