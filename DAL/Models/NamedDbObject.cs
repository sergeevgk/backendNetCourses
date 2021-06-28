namespace DAL.Models
{

	/// <summary>
	/// interface representing objects with property Name for generic FindByName repository method
	/// </summary>
	public interface INamedObject
	{
		public string Name { get; set; }
	}
	public abstract class NamedDbObject : INamedObject
	{
		protected NamedDbObject()
		{
		}

		protected NamedDbObject(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set ; }
	}
}
