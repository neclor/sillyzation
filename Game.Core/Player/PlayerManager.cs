namespace Game.Core;

public class PlayerManager {

	public const int MaxPlayers = 10;

	public IReadOnlyList<Player> Players => _players;

	private readonly List<Player> _players = [];

	public bool TryAddPlayer(Player player) {
		if (_players.Count >= MaxPlayers) return false;
		_players.Add(player);
		return true;
	}

	public bool RemovePlayer(Player player) => _players.Remove(player);
}
