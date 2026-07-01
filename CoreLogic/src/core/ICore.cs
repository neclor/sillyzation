global using PlayerKey = uint;
global using UnitKey = uint;
global using FrontKey = uint;
global using IGameTick = bool;
global using Coord = (uint x, uint y);

using ErrorOr;

namespace CoreLogic;

public interface ICore<TCellKey> where TCellKey : notnull {
	// Game
	ErrorOr<IGameTick> nextGameTick();
	ErrorOr<IGameTick> syncGame();

	// Player
	ErrorOr<IPlayer> getPlayer(PlayerKey playerId);
	Dictionary<uint, IPlayer> getAllPlayers();
	ErrorOr<bool> addPlayer(string name, Color color);
	ErrorOr<bool> kickPlayer(PlayerKey playerId);

	// Cells
	ErrorOr<ICell<TCellKey>> getCell(uint playerId, TCellKey cellId);

	// Unit Queue
	ErrorOr<IUnitQueue<TCellKey>> getUnitQueue(PlayerKey playerId);
	ErrorOr<bool> createUnitQueueGroup(PlayerKey playerId);
	ErrorOr<bool> deployUnitQueueGroup(PlayerKey playerId, uint queueGroupId);
	ErrorOr<bool> addUnitToQueueGroup(PlayerKey playerId);
	ErrorOr<bool> removeUnitToQueueGroup(PlayerKey playerId, UnitKey unitInQueueGroupId);

	// Unit
	ErrorOr<IUnit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId);
	ErrorOr<IEnumerable<IUnit<TCellKey>>> getAllUnits(PlayerKey playerId);
	ErrorOr<bool> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId);
	ErrorOr<bool> assignUnitToFront(PlayerKey playerId, UnitKey unitId, FrontKey frontId);
	ErrorOr<bool> deleteUnit(PlayerKey playerId, UnitKey unitId);

	// Combat
	ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId);

	// Front
	ErrorOr<IFront<TCellKey>> getFront(PlayerKey playerId, FrontKey frontId);
	ErrorOr<bool> createFront(PlayerKey playerId, TCellKey cellId1, TCellKey cellId2);
	ErrorOr<bool> moveFront(PlayerKey playerId, FrontKey frontId, TCellKey cellId, bool side);
}
