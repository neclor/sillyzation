using CoreLogic;
using ErrorOr;

namespace session;

internal class LocalSession<TCellKey> : ISession<TCellKey> where TCellKey : notnull {
	public IPlayer currentPlayer => throw new NotImplementedException();

	public bool gameState => throw new NotImplementedException();

	private ICore<TCellKey> core;

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

		// Dictionary<uint, IPlayer> core_players = core.getAllPlayers();
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
}