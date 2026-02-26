namespace CoreLogic;

internal class Player : IPlayer {
	public uint id { get; }
	public string name { get; }
	public Color color { get; }

	public Player(
		uint id,
		string name,
		Color color
	) {
		this.id = id;
		this.name = name;
		this.color = color;
	}
}
