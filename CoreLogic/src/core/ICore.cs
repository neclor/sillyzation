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
	Dictionary<PlayerKey, IPlayer> getAllPlayers();
	ErrorOr<bool> addPlayer(string name, Color color);
	ErrorOr<bool> kickPlayer(PlayerKey playerId);

	// Cells
	ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId);

	// Unit Queue
	ErrorOr<IEnumerable<IUnitQueue<TCellKey>>> getAllUnitQueue(PlayerKey playerId);
	ErrorOr<IUnitQueue<TCellKey>> getUnitQueue(PlayerKey playerId, QueueKey queueGroupId);
	ErrorOr<bool> createUnitQueueGroup(PlayerKey playerId);
	ErrorOr<bool> deployUnitQueueGroup(PlayerKey playerId, QueueKey queueGroupId);
	ErrorOr<bool> addUnitToQueueGroup(PlayerKey playerId, QueueKey queueGroupId, IUnit<TCellKey> unit);
	ErrorOr<bool> removeUnitToQueueGroup(PlayerKey playerId, UnitKey unitInQueueGroupId);

	// Unit
	ErrorOr<IUnit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId);
	ErrorOr<IEnumerable<IUnit<TCellKey>>> getAllUnits(PlayerKey playerId);
	ErrorOr<bool> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId);
	ErrorOr<bool> deleteUnit(PlayerKey playerId, UnitKey unitId);

	// Combat
	ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId);
}
