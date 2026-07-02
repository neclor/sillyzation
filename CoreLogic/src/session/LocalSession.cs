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

	public IUnit<TCellKey>[] current_player_units => throw new NotImplementedException();

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

	public ErrorOr<bool> addPlayer(string name, Color color) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> kickPlayer(PlayerKey playerId) {
		throw new NotImplementedException();
	}

	public ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId) {
		return core.getCell(playerId, cellId);
	}

	public ErrorOr<IUnitQueue<TCellKey>> getUnitQueue(PlayerKey playerId) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> createUnitQueueGroup(PlayerKey playerId) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> deployUnitQueueGroup(PlayerKey playerId, QueueKey queueGroupId) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> addUnitToQueueGroup(PlayerKey playerId, QueueKey queueGroupId, IUnit<TCellKey> unit) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> removeUnitToQueueGroup(PlayerKey playerId, UnitKey unitInQueueGroupId) {
		throw new NotImplementedException();
	}

	public ErrorOr<IUnit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId) {
		throw new NotImplementedException();
	}

	public ErrorOr<IEnumerable<IUnit<TCellKey>>> getAllUnits(PlayerKey playerId) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> assignUnitToFront(PlayerKey playerId, UnitKey unitId, FrontKey frontId) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> deleteUnit(PlayerKey playerId, UnitKey unitId) {
		throw new NotImplementedException();
	}

	public ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId) {
		throw new NotImplementedException();
	}

	public ErrorOr<IFront<TCellKey>> getFront(PlayerKey playerId, FrontKey frontId) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> createFront(PlayerKey playerId, TCellKey cellId1, TCellKey cellId2) {
		throw new NotImplementedException();
	}

	public ErrorOr<bool> moveFront(PlayerKey playerId, FrontKey frontId, TCellKey cellId, bool side) {
		throw new NotImplementedException();
	}

	public void endTurn() {
		currentPlayerId++;
		if (currentPlayerId.value == players.Length) {
			currentPlayerId = 0;
			// Process Turn
		}
		if (currentPlayer.isAI()) {
			// Process AI Turn actions
			endTurn();
		}
	}
}
