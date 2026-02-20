using CoreLogic.Map;
using ErrorOr;

namespace CoreLogic;

internal class Core : ICore {
	private Map<uint> map;

	public Core() {
		map = new(
			[
				(1, new Cell()),
				(2, new Cell()),
				(3, new Cell()),
				(4, new Cell()),
			],
			[
				(1, 2),
				(2, 3),
				(3, 4),
				(4, 1),
			]
		);
	}

	public ErrorOr<IGame> nextGameTick() {
		return new Game();
	}

	public ErrorOr<IGame> syncGame() {
		return new Game();
	}



	// Player
	public ErrorOr<IPlayer> getPlayer(uint playerId) {
		return new Player();
	}

	public ErrorOr<IEnumerable<IPlayer>> getAllPlayers() {
		return new[] { new Player() };
	}

	public ErrorOr<bool> addPlayer() {
		return true;
	}

	public ErrorOr<bool> kickPlayer() {
		return true;
	}



	// Cells
	public ErrorOr<ICell> getCell(uint playerId, uint cellId) {
		return new Cell();
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