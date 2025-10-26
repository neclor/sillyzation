namespace Game.Server.Lobbies;

public class Lobby {

	public const int MaxPlayers = 20;

	public string Code { get; set; } = "";
	public string OwnerId { get; set; } = "0";
	public int Count => _players.Count;

	private readonly List<string> _players = [];

	public bool AddPlayer(string connectionId) {
		if (_players.Count >= MaxPlayers) return false;
		_players.Add(connectionId);
		return true;
	}

	public bool RemovePlayer(string connectionId) => _players.Remove(connectionId);
}
