global using PlayerKey = uint;
global using CellKey = (uint, uint);

using ErrorOr;

namespace CoreLogic;

public interface ICore {
	// Game
	ErrorOr<IGame> nextGameTick();
	ErrorOr<IGame> syncGame();

	// Player
	ErrorOr<IPlayer> getPlayer(PlayerKey playerId);
	IEnumerable<IPlayer> getAllPlayers();
	ErrorOr<bool> addPlayer(string name, Color color);
	ErrorOr<bool> kickPlayer(PlayerKey playerId);

	// Cells
	ErrorOr<ICell> getCell(uint playerId, CellKey cellId);

	// Unit Queue
	ErrorOr<IUnitQueue> getUnitQueue(PlayerKey playerId);
	ErrorOr<bool> createUnitQueueGroup(PlayerKey playerId);
	ErrorOr<bool> deployUnitQueueGroup(PlayerKey playerId, uint queueGroupId);
	ErrorOr<bool> addUnitToQueueGroup(PlayerKey playerId);
	ErrorOr<bool> removeUnitToQueueGroup(PlayerKey playerId, uint unitInQueueGroupId);

	// Unit
	ErrorOr<IUnit> getUnit(PlayerKey playerId, uint unitId);
	ErrorOr<IEnumerable<IUnit>> getAllUnits(PlayerKey playerId);
	ErrorOr<bool> moveUnit(PlayerKey playerId, uint unitId, CellKey cellId);
	ErrorOr<bool> assignUnitToFront(PlayerKey playerId, uint unitId, uint frontId);
	ErrorOr<bool> deleteUnit(PlayerKey playerId, uint unitId);

	// Combat
	ErrorOr<ICombat> getCombatInfo(uint playerId, uint combatId);

	// Front
	ErrorOr<IFront> getFront(PlayerKey playerId, uint frontId);
	ErrorOr<bool> createFront(PlayerKey playerId, CellKey cellId1, CellKey cellId2);
	ErrorOr<bool> moveFront(PlayerKey playerId, uint frontId, CellKey cellId, bool side);
}
