using CoreLogic;
using ErrorOr;

namespace session;

internal class LocalSession<TCellKey> : ISession<TCellKey> where TCellKey : notnull {
	private PlayerKey currentPlayerId;
	public IPlayer currentPlayer => players[currentPlayerId];
	private readonly IPlayer[] players;
	private readonly Core<TCellKey> core;

	public bool gameState => throw new NotImplementedException();

	uint ISession<TCellKey>.currentPlayerId => currentPlayerId;

	public LocalSession(
		IEnumerable<(ISessionPlayer session, Country country, TCellKey[] start)> players,
		IEnumerable<(TCellKey key, ICell<TCellKey> cell)> cells,
		IEnumerable<(TCellKey key1, TCellKey key2)> connexions
	) {
		Console.WriteLine("Initializing a Multiplayer Local Game");
		core = new Core<TCellKey>(
			players.Select(p => (
				p.country.name,
				p.country.color,
				p.start
			)),
			cells,
			connexions
		);
		this.players = [.. getAllPlayers().Select(v => v.Value).OrderBy(p => p.id)];
		currentPlayerId = 0;
		Console.WriteLine(this.players.Length + " " + currentPlayerId);
	}


	public ErrorOr<ICell<TCellKey>> getCell(uint playerId, TCellKey cellId) {
		return core.getCell(playerId, cellId);
	}

	public ErrorOr<IPlayer> getPlayer(uint playerId) {
		return core.getPlayer(playerId);
	}

	public Dictionary<uint, IPlayer> getAllPlayers() {
		return core.getAllPlayers();
	}

	public void endTurn() {
		currentPlayerId++;
		if (currentPlayerId == players.Length) {
			currentPlayerId = 0;
			// Process Turn
		}
	}
}