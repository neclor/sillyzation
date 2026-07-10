using CoreLogic;
using ErrorOr;

namespace session;

internal class LocalSession<TCellKey> : ISession<TCellKey> where TCellKey : notnull {
	private PlayerKey currentPlayerId;
	public ISessionPlayer currentPlayer => players[currentPlayerId.value];
	private readonly ISessionPlayer[] players;
	private readonly Dictionary<PlayerKey, ISessionPlayer> players_dict;
	private readonly Core<TCellKey> core;

	public bool gameState => throw new NotImplementedException();

	PlayerKey ISession<TCellKey>.currentPlayerId => currentPlayerId;

	public Unit<TCellKey>[] current_player_units => throw new NotImplementedException();

	public LocalSession(
		IEnumerable<(ISessionPlayer player, TCellKey[] start)> players,
		IEnumerable<(TCellKey key, ICell<TCellKey> cell)> cells,
		IEnumerable<(TCellKey key1, TCellKey key2)> connexions
	) {
		Console.WriteLine("Initializing a Multiplayer Local Game");
		core = new Core<TCellKey>(
			players.Select(x => ((IPlayer) x.player, x.start)),
			cells,
			connexions
		);
		this.players = [.. players.Select(e => e.player)];
		players_dict = players.ToDictionary(p => p.player.id, p => p.player);
		currentPlayerId = 0;
	}

	public ErrorOr<ISessionPlayer> getPlayer(PlayerKey playerId) {
		return players[playerId.value].ToErrorOr();
	}

	Dictionary<PlayerKey, ISessionPlayer> ISession<TCellKey>.getAllPlayers() {
		return players_dict;
	}

	public ErrorOr<Success> addPlayer(string name, Color color) {
		throw new NotImplementedException();
	}

	public ErrorOr<Success> kickPlayer(PlayerKey playerId) {
		throw new NotImplementedException();
	}



	// Cells

	public ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId) {
		return core.getCell(playerId, cellId);
	}



	// Unit Queue

	public ErrorOr<IUnitQueue<TCellKey>[]> getAllUnitQueue(PlayerKey playerId) {
		return core.getAllUnitQueue(playerId);
	}

	public ErrorOr<QueueKey[]> getAllUnitQueueId(PlayerKey playerId) {
		return core.getAllUnitQueueId(playerId);
	}

	public ErrorOr<IUnitQueue<TCellKey>> getUnitQueue(PlayerKey playerId, QueueKey queueGroupId) {
		return core.getUnitQueue(playerId, queueGroupId);
	}

	public ErrorOr<QueueUnit<TCellKey>[]> getAllUnitInQueue(PlayerKey playerId, QueueKey queueGroupId) {
		return core.getAllUnitInQueue(playerId, queueGroupId);
	}

	public ErrorOr<Success> createUnitQueueGroup(PlayerKey playerId) {
		return core.createUnitQueue(playerId);
	}

	public ErrorOr<Success> deployUnitQueueGroup(PlayerKey playerId, QueueKey queueGroupId, TCellKey pos) {
		return core.deployUnitQueue(playerId, queueGroupId, pos);
	}

	public ErrorOr<Success> addUnitToQueue(PlayerKey playerId, QueueKey queueGroupId, QueueUnit<TCellKey> unit) {
		return core.addUnitToQueue(playerId, queueGroupId, unit);
	}

	public ErrorOr<Success> deleteUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit) {
		var x = core.deleteUnitFromQueue(playerId, queueGroupId, unit);
		Console.WriteLine(x);
		return x;
	}

	public ErrorOr<Success> deployUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit, TCellKey pos) {
		return core.deployUnitFromQueue(playerId, queueGroupId, unit, pos);
	}



	// Unit

	public ErrorOr<MapUnit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId) {
		throw new NotImplementedException();
	}

	public ErrorOr<MapUnit<TCellKey>[]> getAllUnits(PlayerKey playerId) {
		return core.getAllUnits(playerId);
	}

	public ErrorOr<Success> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId) {
		throw new NotImplementedException();
	}

	public ErrorOr<Success> deleteUnit(PlayerKey playerId, UnitKey unitId) {
		throw new NotImplementedException();
	}

	public ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId) {
		throw new NotImplementedException();
	}



	// Combat

	public void endTurn() {
		currentPlayerId++;
		if (currentPlayerId.value == players.Length) {
			currentPlayerId = 0;
			// Process Turn
			_ = core.nextGameTick();
		}
		if (currentPlayer.isAI()) {
			// Process AI Turn actions
			endTurn();
		}
	}
}
