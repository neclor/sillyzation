namespace Core;

public class Lobby {

	public const int MaxPlayers = 10;

	public Player Host { get; private set; } = new();

	public IReadOnlyList<Player> Players => _players;
	private readonly List<Player> _players;

	public Lobby() => _players = [Host];

	public bool TrySetHost(Player player) {
		if (!_players.Contains(player)) return false;
		Host = player;
		return true;
	}

	public bool TryAddPlayer(Player player) {
		if (_players.Count >= MaxPlayers) return false;
		_players.Add(player);
		return true;
	}

	public bool RemovePlayer(Player player) {
		if (player == Host) return false;
		return _players.Remove(player);
	}

	public Player? GetPlayerFromId(Guid id) => _players.FirstOrDefault(p => p.Id == id);
}
