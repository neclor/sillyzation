namespace CoreLogic;

internal class Player : IPlayer {
	public PlayerKey id { get; }
	public string name { get; }
	public Color color { get; }

	public Player(
		PlayerKey id,
		string name,
		Color color
	) {
		this.id = id;
		this.name = name;
		this.color = color;
	}
}
