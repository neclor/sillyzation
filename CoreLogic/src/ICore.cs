using ErrorOr;

namespace CoreLogic;

public interface ICore<TKey> {
	// Game
	ErrorOr<IGame> nextGameTick();
	ErrorOr<IGame> syncGame();

	// Player
	ErrorOr<IPlayer> getPlayer(uint playerId);
	IEnumerable<IPlayer> getAllPlayers();
	ErrorOr<bool> addPlayer();
	ErrorOr<bool> kickPlayer();

	// Cells
	ErrorOr<ICell<TKey>> getCell(uint playerId, uint cellId);

	// Unit Queue
	ErrorOr<IUnitQueue> getUnitQueue(uint playerId);
	ErrorOr<bool> createUnitQueueGroup(uint playerId);
	ErrorOr<bool> deployUnitQueueGroup(uint playerId, uint queueGroupId);
	ErrorOr<bool> addUnitToQueueGroup(uint playerId);
	ErrorOr<bool> removeUnitToQueueGroup(uint playerId, uint unitInQueueGroupId);

	// Unit
	ErrorOr<IUnit> getUnit(uint playerId, uint unitId);
	ErrorOr<IEnumerable<IUnit>> getAllUnits(uint playerId);
	ErrorOr<bool> moveUnit(uint playerId, uint unitId, uint cellId);
	ErrorOr<bool> assignUnitToFront(uint playerId, uint unitId, uint frontId);
	ErrorOr<bool> deleteUnit(uint playerId, uint unitId);

	// Combat
	ErrorOr<ICombat> getCombatInfo(uint playerId, uint combatId);

	// Front
	ErrorOr<IFront> getFront(uint playerId, uint frontId);
	ErrorOr<bool> createFront(uint playerId, uint cellId1, uint cellId2);
	ErrorOr<bool> moveFront(uint playerId, uint frontId, uint cellId, bool side);
}
