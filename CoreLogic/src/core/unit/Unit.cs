namespace CoreLogic;

public class Unit<TCellKey> where TCellKey : notnull {
	private static uint id_counter = 1;
	public UnitKey id { get; }
	public string name { get; }
	public uint baseHealth { get; }
	public uint health { get; }
	public uint speed { get; }
	public PlayerKey owner { get; }

	public Unit(
		string name,
		uint health,
		uint speed,
		PlayerKey owner
	) {
		id = id_counter++;
		this.name = name;
		baseHealth = health;
		this.health = health;
		this.speed = speed;
		this.owner = owner;
	}

	public Unit(
		UnitKey id,
		string name,
		uint baseHealth,
		uint health,
		uint speed,
		PlayerKey owner
	) {
		this.id = id;
		this.name = name;
		this.baseHealth = baseHealth;
		this.health = health;
		this.speed = speed;
		this.owner = owner;
	}

	public QueueUnit<TCellKey> toQueue() {
		return new QueueUnit<TCellKey>(id, name, baseHealth, health, speed, owner);
	}
}

public class QueueUnit<TCellKey> : Unit<TCellKey> where TCellKey : notnull {
	public uint progress { get; }

	public QueueUnit(
		UnitKey id,
		string name,
		uint baseHealth,
		uint health,
		uint speed,
		PlayerKey owner
	) : base(id, name, baseHealth, health, speed, owner) {
		progress = 0;
	}

	public MapUnit<TCellKey> deploy(TCellKey position) {
		return new MapUnit<TCellKey>(id, name, baseHealth, health, speed, owner, position);
	}
}

public class MapUnit<TCellKey> : Unit<TCellKey> where TCellKey : notnull {
	public TCellKey position { get; }

	public MapUnit(
		UnitKey id,
		string name,
		uint baseHealth,
		uint health,
		uint speed,
		PlayerKey owner,
		TCellKey position
	) : base(id, name, baseHealth, health, speed, owner) {
		this.position = position;
	}
}