using CoreLogic.Map;
using ErrorOr;

namespace CoreLogic;

internal class Core<TKey> : ICore<TKey> where TKey : notnull, IEquatable<TKey> {
	// private readonly Map<uint> map = new([], []);
	private readonly IEnumerable<IPlayer> players;

	public Core(
		IEnumerable<PlayerInit> players
	) {
		uint id = 1;
		this.players = players.Select(player => new Player(
			id++,
			player.name,
			player.color
		));
	}

	public ErrorOr<IGame> nextGameTick() {
		return new Game();
	}

	public ErrorOr<IGame> syncGame() {
		return new Game();
	}



	// Player
	public ErrorOr<IPlayer> getPlayer(uint playerId) {
		IPlayer? player = players.First(player => player.id == playerId);
		if (player == null) {
			return Error.NotFound();
		}
		return player.ToErrorOr();
	}

	public IEnumerable<IPlayer> getAllPlayers() {
		return players;
	}

	public ErrorOr<bool> addPlayer() {
		return true;
	}

	public ErrorOr<bool> kickPlayer() {
		return true;
	}



	// Cells
	public ErrorOr<ICell<TKey>> getCell(uint playerId, uint cellId) {
		throw new NotImplementedException();
		// return new Cell<TKey>(null, null, Terrain.Plain);
	}



	// Unit Queue
	public ErrorOr<IUnitQueue> getUnitQueue(uint playerId) {
		return new UnitQueue();
	}

	public ErrorOr<bool> createUnitQueueGroup(uint playerId) {
		return true;
	}

	public ErrorOr<bool> deployUnitQueueGroup(uint playerId, uint queueGroupId) {
		return true;
	}

	public ErrorOr<bool> addUnitToQueueGroup(uint playerId) {
		return true;
	}

	public ErrorOr<bool> removeUnitToQueueGroup(uint playerId, uint unitInQueueGroupId) {
		return true;
	}



	// Unit
	public ErrorOr<IUnit> getUnit(uint playerId, uint unitId) {
		return new Unit();
	}

	public ErrorOr<IEnumerable<IUnit>> getAllUnits(uint playerId) {
		return new[] { new Unit() };
	}

	public ErrorOr<bool> moveUnit(uint playerId, uint unitId, uint cellId) {
		return true;
	}

	public ErrorOr<bool> assignUnitToFront(uint playerId, uint unitId, uint frontId) {
		return true;
	}

	public ErrorOr<bool> deleteUnit(uint playerId, uint unitId) {
		return true;
	}



	// Combat
	public ErrorOr<ICombat> getCombatInfo(uint playerId, uint combatId) {
		return new Combat();
	}



	// Front
	public ErrorOr<IFront> getFront(uint playerId, uint frontId) {
		return new Front();
	}

	public ErrorOr<bool> createFront(uint playerId, uint cellId1, uint cellId2) {
		return true;
	}

	public ErrorOr<bool> moveFront(uint playerId, uint frontId, uint cellId, bool side) {
		return true;
	}
}