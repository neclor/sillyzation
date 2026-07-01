using CoreLogic;

namespace session;

internal class HumanPlayer : ISessionPlayer {
	public PlayerKey id { get; }
	public Country country { get; }
	public string name => country.name;
	public Color color => country.color;
	public bool isAI() => false;

	public HumanPlayer(
		PlayerKey id,
		Country country
	) {
		this.id = id;
		this.country = country;
	}
}
