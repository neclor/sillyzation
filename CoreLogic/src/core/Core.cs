using ErrorOr;
using QuikGraph.Collections;

namespace CoreLogic;

internal class Core<TCellKey> : ICore<TCellKey> where TCellKey : notnull {

	private readonly Map<TCellKey> map;
	private readonly Game game;

	private readonly Dictionary<PlayerKey, (
		IPlayer player,
		Dictionary<QueueKey, IUnitQueue<TCellKey>> queues
	)> players;

	public Core(
		IEnumerable<(IPlayer player, TCellKey[] ownership)> players,
		IEnumerable<(TCellKey key, ICell<TCellKey> cell)> cells,
		IEnumerable<(TCellKey key1, TCellKey key2)> connexions
	) {
		game = new(players.Select(p => p.player));

		IEnumerable<(uint playerId, TCellKey[] cells)> ownerships = players
			.Select((x, _) => (x.player.id.value, x.ownership));

		// Map
		map = new(cells, connexions, ownerships);

		this.players = players
			.ToDictionary(
				x => x.player.id,
				x => (x.player, new Dictionary<QueueKey, IUnitQueue<TCellKey>>())
			);
	}

	public ErrorOr<IGameTick> nextGameTick() {
		return true.ToErrorOr();
	}

	public ErrorOr<IGameTick> syncGame() {
		return true.ToErrorOr();
	}



	// Player
	public ErrorOr<IPlayer> getPlayer(PlayerKey playerId) {
		return game.getPlayer(playerId);
	}

	public Dictionary<PlayerKey, IPlayer> getAllPlayers() {
		return game.getAllPlayers();
	}

	public ErrorOr<Success> kickPlayer(PlayerKey playerId) {
		return game.kickPlayer(playerId);
	}



	// Cells
	public ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId) {
		// TODO Add player protection
		return map.getCell(cellId);
	}



	// Unit Queue
	// ErrorOr<IEnumerable<IUnitQueue<TCellKey>>> getAllUnitQueue(PlayerKey playerId)
	public ErrorOr<IUnitQueue<TCellKey>[]> getAllUnitQueue(PlayerKey playerId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		return player.queues
			.Select(e => e.Value)
			.ToArray()
			.ToErrorOr();
	}

	public ErrorOr<QueueKey[]> getAllUnitQueueId(PlayerKey playerId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		Console.WriteLine($"Queue for player : {playerId}");
		foreach (var q in player.queues) {
			Console.WriteLine(q.Key);
		}
		var res = player.queues
			.Select(e => new QueueKey(e.Value.id))
			.ToArray()
			.ToErrorOr();
		foreach (var x in res.Value) {
			Console.WriteLine(x);
		}
		return res;
	}

	public ErrorOr<IUnitQueue<TCellKey>> getUnitQueue(PlayerKey playerId, QueueKey queueGroupId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.queues.TryGetValue(queueGroupId, out IUnitQueue<TCellKey>? queue)) {
			return Error.NotFound();
		}
		return queue.ToErrorOr();
	}

	public ErrorOr<Success> createUnitQueue(PlayerKey playerId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		IUnitQueue<TCellKey> newQueue = new UnitQueue<TCellKey>();
		if (!player.queues.TryAdd(newQueue.id, newQueue)) {
			return Error.Conflict();
		}
		return Result.Success;
	}

	public ErrorOr<Success> deployUnitQueue(PlayerKey playerId, QueueKey queueGroupId, TCellKey pos) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		// TODO deploy units
		if (!player.queues.Remove(queueGroupId)) {
			return Error.NotFound();
		}
		return Result.Success;
	}

	public ErrorOr<Success> addUnitToQueue(PlayerKey playerId, QueueKey queueGroupId, IUnit<TCellKey> unit) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.queues.TryGetValue(queueGroupId, out IUnitQueue<TCellKey>? queue)) {
			return Error.NotFound();
		}
		return queue.addUnit(unit);
	}

	public ErrorOr<Success> deleteUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.queues.TryGetValue(queueGroupId, out IUnitQueue<TCellKey>? queue)) {
			return Error.NotFound();
		}
		return queue.removeUnit(unit);
	}

	public ErrorOr<Success> deployUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit, TCellKey pos) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.queues.TryGetValue(queueGroupId, out IUnitQueue<TCellKey>? queue)) {
			return Error.NotFound();
		}
		if (queue.removeUnit(unit).IsError) {
			return Error.NotFound();
		}
		// TODO place the unit
		return Result.Success;
	}



	// Unit
	public ErrorOr<IUnit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId) {
		throw new NotImplementedException();
	}

	public ErrorOr<IEnumerable<IUnit<TCellKey>>> getAllUnits(PlayerKey playerId) {
		throw new NotImplementedException();
	}

	public ErrorOr<Success> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId) {
		return Result.Success;
	}

	public ErrorOr<Success> deleteUnit(PlayerKey playerId, UnitKey unitId) {
		return Result.Success;
	}



	// Combat
	public ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId) {
		return new Combat<TCellKey>();
	}
}
