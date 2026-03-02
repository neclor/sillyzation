global using PlayerKey = uint;
global using CellKey = (uint, uint);
global using UnitKey = uint;
global using FrontKey = uint;
global using IGameTick = bool;


using ErrorOr;

namespace CoreLogic;

public interface ICore {
	// Game
	ErrorOr<IGameTick> nextGameTick();
	ErrorOr<IGameTick> syncGame();

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
	ErrorOr<bool> removeUnitToQueueGroup(PlayerKey playerId, UnitKey unitInQueueGroupId);

	// Unit
	ErrorOr<IUnit> getUnit(PlayerKey playerId, UnitKey unitId);
	ErrorOr<IEnumerable<IUnit>> getAllUnits(PlayerKey playerId);
	ErrorOr<bool> moveUnit(PlayerKey playerId, UnitKey unitId, CellKey cellId);
	ErrorOr<bool> assignUnitToFront(PlayerKey playerId, UnitKey unitId, FrontKey frontId);
	ErrorOr<bool> deleteUnit(PlayerKey playerId, UnitKey unitId);

	// Combat
	ErrorOr<ICombat> getCombatInfo(PlayerKey playerId, uint combatId);

	// Front
	ErrorOr<IFront> getFront(PlayerKey playerId, FrontKey frontId);
	ErrorOr<bool> createFront(PlayerKey playerId, CellKey cellId1, CellKey cellId2);
	ErrorOr<bool> moveFront(PlayerKey playerId, FrontKey frontId, CellKey cellId, bool side);
}
