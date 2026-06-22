using ErrorOr;

namespace CoreLogic;

internal class Core : ICore {

	private readonly Map map;
	private readonly Game game;

	public Core(
		IEnumerable<(string name, Color color, CellKey[] ownership)> players,
		IEnumerable<(CellKey key, ICell cell)> cells,
		IEnumerable<(CellKey key1, CellKey key2)> connexions
	) {
		game = new(players.Select(p => (p.name, p.color)));

		IEnumerable<(uint playerId, CellKey[] cells)> ownerships = players
			.Join(
				game.getAllPlayers(),
				playerInput => playerInput.name,
				playerDict => playerDict.Value.name,
				(playerInput, playerDict) => (playerId: playerDict.Key, cells: playerInput.ownership)
			);

		// Map
		map = new(cells, connexions, ownerships);
	}

	public ErrorOr<IGameTick> nextGameTick() {
		return true.ToErrorOr();
	}

	public ErrorOr<IGameTick> syncGame() {
		return true.ToErrorOr();
	}



	// Player
	public ErrorOr<IPlayer> getPlayer(PlayerKey playerId) {
		return game.getPlayer(playerId);
	}

	public Dictionary<uint, IPlayer> getAllPlayers() {
		return game.getAllPlayers();
	}

	public ErrorOr<bool> addPlayer(string name, Color color) {
		return game.addPlayer(name, color);
	}

	public ErrorOr<bool> kickPlayer(PlayerKey playerId) {
		return game.kickPlayer(playerId);
	}



	// Cells
	public ErrorOr<ICell> getCell(PlayerKey playerId, CellKey cellId) {
		// TODO Add player protection
		return map.getCell(cellId);
	}



	// Unit Queue
	public ErrorOr<IUnitQueue> getUnitQueue(PlayerKey playerId) {
		return new UnitQueue();
	}

	public ErrorOr<bool> createUnitQueueGroup(PlayerKey playerId) {
		return true;
	}

	public ErrorOr<bool> deployUnitQueueGroup(PlayerKey playerId, uint queueGroupId) {
		return true;
	}

	public ErrorOr<bool> addUnitToQueueGroup(PlayerKey playerId) {
		return true;
	}

	public ErrorOr<bool> removeUnitToQueueGroup(PlayerKey playerId, uint unitInQueueGroupId) {
		return true;
	}



	// Unit
	public ErrorOr<IUnit> getUnit(PlayerKey playerId, uint unitId) {
		return new Unit();
	}

	public ErrorOr<IEnumerable<IUnit>> getAllUnits(PlayerKey playerId) {
		return new[] { new Unit() };
	}

	public ErrorOr<bool> moveUnit(PlayerKey playerId, uint unitId, CellKey cellId) {
		return true;
	}

	public ErrorOr<bool> assignUnitToFront(PlayerKey playerId, uint unitId, uint frontId) {
		return true;
	}

	public ErrorOr<bool> deleteUnit(PlayerKey playerId, uint unitId) {
		return true;
	}



	// Combat
	public ErrorOr<ICombat> getCombatInfo(PlayerKey playerId, uint combatId) {
		return new Combat();
	}



	// Front
	public ErrorOr<IFront> getFront(PlayerKey playerId, uint frontId) {
		return new Front();
	}

	public ErrorOr<bool> createFront(PlayerKey playerId, CellKey cellId1, CellKey cellId2) {
		return true;
	}

	public ErrorOr<bool> moveFront(PlayerKey playerId, uint frontId, CellKey cellId, bool side) {
		return true;
	}
}