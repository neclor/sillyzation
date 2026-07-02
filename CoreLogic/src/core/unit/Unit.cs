namespace CoreLogic;

internal class Infantry<TCellKey> : IUnit<TCellKey> {
	public UnitKey id => new(0);
	public string name => "Infantry Division";
	public uint baseHealth => 50;
	public uint health => 50;
	public uint speed => 1;
	public PlayerKey owner { get; }
	public TCellKey position { get; }

	public Infantry(TCellKey position, PlayerKey owner) {
		this.position = position;
		this.owner = owner;
	}
}

internal class Tank<TCellKey> : IUnit<TCellKey> {
	public UnitKey id => new(0);
	public string name => "Tank Division";
	public uint baseHealth => 200;
	public uint health => 200;
	public uint speed => 1;
	public PlayerKey owner { get; }
	public TCellKey position { get; }

	public Tank(TCellKey position, PlayerKey owner) {
		this.position = position;
		this.owner = owner;
	}
}