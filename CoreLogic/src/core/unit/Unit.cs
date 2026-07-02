namespace CoreLogic;

internal class Infantry<TCellKey> : IUnit<TCellKey> where TCellKey : notnull {
	public UnitKey id { get; } = 0;
	public string name => "Infantry Division";
	public uint baseHealth => 50;
	public uint health { get; } = 50;
	public uint speed => 1;
	public PlayerKey owner { get; }
	public TCellKey? position { get; }

	public Infantry(PlayerKey owner, TCellKey? position = default) {
		this.position = position;
		this.owner = owner;
	}
}

internal class Tank<TCellKey> : IUnit<TCellKey> where TCellKey : notnull {
	public UnitKey id { get; } = 0;
	public string name => "Tank Division";
	public uint baseHealth => 200;
	public uint health { get; } = 200;
	public uint speed => 2;
	public PlayerKey owner { get; }
	public TCellKey? position { get; }

	public Tank(PlayerKey owner, TCellKey? position = default) {
		this.position = position;
		this.owner = owner;
	}
}