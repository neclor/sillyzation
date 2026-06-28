using CoreLogic;
using ErrorOr;

namespace session;

internal class LocalSession : ISession {
	public IPlayer currentPlayer => throw new NotImplementedException();

	public bool gameState => throw new NotImplementedException();

	private ICore core;

	public LocalSession(
		IEnumerable<(ISessionPlayer session, Country country, CellKey[] start)> players,
		IEnumerable<(CellKey key, ICell cell)> cells,
		IEnumerable<(CellKey key1, CellKey key2)> connexions
	) {
		Console.WriteLine("Initializing a Multiplayer Local Game");
		core = new Core(
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


	public ErrorOr<ICell> getCell(uint playerId, CellKey cellId) {
		return core.getCell(playerId, cellId);
	}

	public ErrorOr<IPlayer> getPlayer(uint playerId) {
		return core.getPlayer(playerId);
	}

	public Dictionary<uint, IPlayer> getAllPlayers() {
		return core.getAllPlayers();
	}
}