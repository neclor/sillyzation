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
			.Select((x, _) => (x.player.id.value, x.ownership));

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

	public Dictionary<PlayerKey, IPlayer> getAllPlayers() {
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
	public ErrorOr<IUnitQueue<TCellKey>> getUnitQueue(PlayerKey playerId) {
		return new UnitQueue<TCellKey>();
	}

	public ErrorOr<bool> createUnitQueueGroup(PlayerKey playerId) {
		return true;
	}

	public ErrorOr<bool> deployUnitQueueGroup(PlayerKey playerId, QueueKey queueGroupId) {
		return true;
	}

	public ErrorOr<bool> addUnitToQueueGroup(PlayerKey playerId, QueueKey queueGroupId, IUnit<TCellKey> unit) {
		return true;
	}

	public ErrorOr<bool> removeUnitToQueueGroup(PlayerKey playerId, UnitKey unitInQueueGroupId) {
		return true;
	}



	// Unit
	public ErrorOr<IUnit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId) {
		throw new NotImplementedException();
		// return new Infantry<TCellKey>();
	}

	public ErrorOr<IEnumerable<IUnit<TCellKey>>> getAllUnits(PlayerKey playerId) {
		throw new NotImplementedException();
		// return new[] { new Unit<TCellKey>() };
	}

	public ErrorOr<bool> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId) {
		return true;
	}

	public ErrorOr<bool> assignUnitToFront(PlayerKey playerId, UnitKey unitId, FrontKey frontId) {
		return true;
	}

	public ErrorOr<bool> deleteUnit(PlayerKey playerId, UnitKey unitId) {
		return true;
	}



	// Combat
	public ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId) {
		return new Combat<TCellKey>();
	}



	// Front
	public ErrorOr<IFront<TCellKey>> getFront(PlayerKey playerId, FrontKey frontId) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> createFront(PlayerKey playerId, TCellKey cellId1, TCellKey cellId2) {
		return true;
	}

	public ErrorOr<bool> moveFront(PlayerKey playerId, FrontKey frontId, TCellKey cellId, bool side) {
		return true;
	}
}
