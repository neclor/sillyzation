using CoreLogic;
using ErrorOr;

namespace session;

internal interface ISession<TCellKey> where TCellKey : notnull {
	PlayerKey currentPlayerId { get; }
	ISessionPlayer currentPlayer { get; }
	Unit<TCellKey>[] current_player_units { get; }
	bool gameState { get; }

	// Player
	ErrorOr<ISessionPlayer> getPlayer(PlayerKey playerId);
	Dictionary<PlayerKey, ISessionPlayer> getAllPlayers();
	ErrorOr<Success> addPlayer(string name, Color color);
	ErrorOr<Success> kickPlayer(PlayerKey playerId);

	// Cells
	ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId);

	// Unit Queue
	ErrorOr<IUnitQueue<TCellKey>[]> getAllUnitQueue(PlayerKey playerId);
	ErrorOr<QueueKey[]> getAllUnitQueueId(PlayerKey playerId);
	ErrorOr<IUnitQueue<TCellKey>> getUnitQueue(PlayerKey playerId, QueueKey queueGroupId);
	ErrorOr<QueueUnit<TCellKey>[]> getAllUnitInQueue(PlayerKey playerId, QueueKey queueGroupId);
	ErrorOr<Success> createUnitQueueGroup(PlayerKey playerId);
	ErrorOr<Success> deployUnitQueueGroup(PlayerKey playerId, QueueKey queueGroupId, TCellKey pos);
	ErrorOr<Success> addUnitToQueue(PlayerKey playerId, QueueKey queueGroupId, QueueUnit<TCellKey> unit);
	ErrorOr<Success> deleteUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit);
	ErrorOr<Success> deployUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit, TCellKey pos);

	// Unit
	ErrorOr<MapUnit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId);
	ErrorOr<MapUnit<TCellKey>[]> getAllUnits(PlayerKey playerId);
	ErrorOr<Success> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId);
	ErrorOr<Success> deleteUnit(PlayerKey playerId, UnitKey unitId);

	// Combat
	ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId);

	void endTurn();
}