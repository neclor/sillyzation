using ErrorOr;

namespace CoreLogic;

internal class Core<TCellKey> : ICore<TCellKey> where TCellKey : notnull {

	private readonly Map<TCellKey> map;
	private readonly Game game;

	public Core(
		IEnumerable<(IPlayer player, TCellKey[] ownership)> players,
		IEnumerable<(TCellKey key, ICell<TCellKey> cell)> cells,
		IEnumerable<(TCellKey key1, TCellKey key2)> connexions
	) {
		game = new(players.Select(p => p.player));

		IEnumerable<(uint playerId, TCellKey[] cells)> ownerships = players
			.Select((x, _) => (x.player.id, x.ownership));

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
	public ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId) {
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

	public ErrorOr<bool> moveUnit(PlayerKey playerId, uint unitId, TCellKey cellId) {
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

	public ErrorOr<bool> createFront(PlayerKey playerId, TCellKey cellId1, TCellKey cellId2) {
		return true;
	}

	public ErrorOr<bool> moveFront(PlayerKey playerId, uint frontId, TCellKey cellId, bool side) {
		return true;
	}
}