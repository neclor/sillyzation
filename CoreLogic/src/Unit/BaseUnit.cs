namespace Game;

public class BaseUnit {
	public uint BaseHealth { get; }
	public uint Health { get; }
	public uint Speed { get; }

	public BaseUnit(
		uint BaseHealth,
		uint Health,
		uint Speed
	) {
		this.BaseHealth = BaseHealth;
		this.Health = Health;
		this.Speed = Speed;
	}
}
