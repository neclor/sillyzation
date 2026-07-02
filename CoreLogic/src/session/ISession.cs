using CoreLogic;
using ErrorOr;

namespace session;

internal interface ISession<TCellKey> where TCellKey : notnull {
	PlayerKey currentPlayerId { get; }
	ISessionPlayer currentPlayer { get; }
	IUnit<TCellKey>[] current_player_units { get; }
	bool gameState { get; }

	// Player
	ErrorOr<ISessionPlayer> getPlayer(PlayerKey playerId);
	Dictionary<PlayerKey, ISessionPlayer> getAllPlayers();
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
	ErrorOr<bool> assignUnitToFront(PlayerKey playerId, UnitKey unitId, FrontKey frontId);
	ErrorOr<bool> deleteUnit(PlayerKey playerId, UnitKey unitId);

	// Combat
	ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId);

	// Front
	ErrorOr<IFront<TCellKey>> getFront(PlayerKey playerId, FrontKey frontId);
	ErrorOr<bool> createFront(PlayerKey playerId, TCellKey cellId1, TCellKey cellId2);
	ErrorOr<bool> moveFront(PlayerKey playerId, FrontKey frontId, TCellKey cellId, bool side);

	void endTurn();
}