
using System.Collections.Generic;

namespace DAL.Models
{
	public class Factory : NamedDbObject
	{
		public Factory() : base()
		{
		}

		public Factory(int id, string name, string description) : base(id, name)
		{
			Description = description;
		}
		public string Description { get; set; }
		public IList<Unit> Units { get; set; }

		public override string ToString()
		{
			return $"[Id: {Id}, Name: {Name}, Description: {Description}]";
		}
	}
}
