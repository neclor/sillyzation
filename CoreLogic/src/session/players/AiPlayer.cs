using CoreLogic;

namespace session;

internal class AiPlayer : ISessionPlayer {
	public PlayerKey id { get; }
	public Country country { get; }
	public string name => country.name;
	public Color color => country.color;
	public bool isAI() => true;

	public AiPlayer(
		PlayerKey id,
		Country country
	) {
		this.id = id;
		this.country = country;
	}
}
