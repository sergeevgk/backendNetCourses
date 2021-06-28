namespace DAL.Models
{
	public class Tank : NamedDbObject
	{
		public Tank() : base()
		{
		}

		public Tank(int id, string name, int volume, int maxVolume, int unitId) : base(id, name)
		{
			Volume = volume;
			MaxVolume = maxVolume;
			UnitId = unitId;
		}
		public int Volume { get; set; }
		public int MaxVolume { get; set; }
		public int UnitId { get; set; }
		public Unit Unit {get; set;}
		public override string ToString()
		{
			return $"[Id: {Id}, Name: {Name}, Volume: {Volume}, MaxVolume: {MaxVolume}, UnitId: {UnitId}]";
		}
	}
}
