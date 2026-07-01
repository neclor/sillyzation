using CoreLogic;
using ErrorOr;

namespace session;

internal class LocalSession<TCellKey> : ISession<TCellKey> where TCellKey : notnull {
	private PlayerKey currentPlayerId;
	public ISessionPlayer currentPlayer => players[currentPlayerId];
	private readonly ISessionPlayer[] players;
	private readonly Dictionary<PlayerKey, ISessionPlayer> players_dict;
	private readonly Core<TCellKey> core;

	public bool gameState => throw new NotImplementedException();

	uint ISession<TCellKey>.currentPlayerId => currentPlayerId;

	public LocalSession(
		IEnumerable<(ISessionPlayer player, TCellKey[] start)> players,
		IEnumerable<(TCellKey key, ICell<TCellKey> cell)> cells,
		IEnumerable<(TCellKey key1, TCellKey key2)> connexions
	) {
		Console.WriteLine("Initializing a Multiplayer Local Game");
		core = new Core<TCellKey>(
			players.Select(x => ((IPlayer) x.player, x.start)),
			cells,
			connexions
		);
		this.players = [.. players.Select(e => e.player)];
		players_dict = players.ToDictionary(p => p.player.id, p => p.player);
		currentPlayerId = 0;
	}


	public ErrorOr<ICell<TCellKey>> getCell(uint playerId, TCellKey cellId) {
		return core.getCell(playerId, cellId);
	}

	public ErrorOr<ISessionPlayer> getPlayer(uint playerId) {
		return players[playerId].ToErrorOr();
	}

	public Dictionary<PlayerKey, ISessionPlayer> getAllPlayers() {
		return players_dict;
	}

	public void endTurn() {
		currentPlayerId++;
		if (currentPlayerId == players.Length) {
			currentPlayerId = 0;
			// Process Turn
		}
		if (currentPlayer.isAI()) {
			// Process AI Turn actions
			endTurn();
		}
	}
}