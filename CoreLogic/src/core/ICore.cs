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
	ErrorOr<Success> kickPlayer(PlayerKey playerId);

	// Cells
	ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId);

	// Unit Queue
	ErrorOr<IUnitQueue<TCellKey>[]> getAllUnitQueue(PlayerKey playerId);
	ErrorOr<IUnitQueue<TCellKey>> getUnitQueue(PlayerKey playerId, QueueKey queueGroupId);
	ErrorOr<QueueUnit<TCellKey>[]> getAllUnitInQueue(PlayerKey playerId, QueueKey queueGroupId);
	ErrorOr<Success> createUnitQueue(PlayerKey playerId);
	ErrorOr<Success> deployUnitQueue(PlayerKey playerId, QueueKey queueGroupId, TCellKey pos);
	ErrorOr<Success> addUnitToQueue(PlayerKey playerId, QueueKey queueGroupId, QueueUnit<TCellKey> unit);
	ErrorOr<Success> deleteUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit);
	ErrorOr<Success> deployUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit, TCellKey pos);

	// Unit
	ErrorOr<Unit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId);
	ErrorOr<Unit<TCellKey>[]> getAllUnits(PlayerKey playerId);
	ErrorOr<Success> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId);
	ErrorOr<Success> deleteUnit(PlayerKey playerId, UnitKey unitId);

	// Combat
	ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId);
}
