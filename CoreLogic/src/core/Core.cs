using ErrorOr;
using QuikGraph.Collections;

namespace CoreLogic;

internal class Core<TCellKey> : ICore<TCellKey> where TCellKey : notnull {

	private readonly Map<TCellKey> map;

	private readonly Dictionary<PlayerKey, (
		IPlayer player,
		Dictionary<QueueKey, IUnitQueue<TCellKey>> queues,
		Dictionary<UnitKey, MapUnit<TCellKey>> units
	)> players;

	public Core(
		IEnumerable<(IPlayer player, TCellKey[] ownership)> players,
		IEnumerable<(TCellKey key, ICell<TCellKey> cell)> cells,
		IEnumerable<(TCellKey key1, TCellKey key2)> connexions
	) {
		IEnumerable<(uint playerId, TCellKey[] cells)> ownerships = players
			.Select((x, _) => (x.player.id.value, x.ownership));

		// Map
		map = new(cells, connexions, ownerships);

		this.players = players
			.ToDictionary(
				x => x.player.id,
				x => (
					x.player,
					new Dictionary<QueueKey, IUnitQueue<TCellKey>>(),
					new Dictionary<UnitKey, MapUnit<TCellKey>>()
				)
			);
	}

	public ErrorOr<IGameTick> nextGameTick() {
		foreach ((_, var player) in players) {
			foreach ((_, var queue) in player.queues) {
				_ = queue.tick();
			}
		}
		return true.ToErrorOr();
	}

	public ErrorOr<IGameTick> syncGame() {
		return true.ToErrorOr();
	}



	// Player
	public ErrorOr<IPlayer> getPlayer(PlayerKey playerId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		return player.player.ToErrorOr();
	}

	public Dictionary<PlayerKey, IPlayer> getAllPlayers() {
		throw new NotImplementedException();
	}

	public ErrorOr<Success> kickPlayer(PlayerKey playerId) {
		throw new NotImplementedException();
	}



	// Cells
	public ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId) {
		// TODO Add player protection
		return map.getCell(cellId);
	}



	// Unit Queue
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

	public ErrorOr<QueueUnit<TCellKey>[]> getAllUnitInQueue(PlayerKey playerId, QueueKey queueGroupId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.queues.TryGetValue(queueGroupId, out IUnitQueue<TCellKey>? queue)) {
			return Error.NotFound();
		}
		return queue.getUnits();
	}

	public ErrorOr<Success> createUnitQueue(PlayerKey playerId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		UnitQueue<TCellKey> newQueue = new();
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

	public ErrorOr<Success> addUnitToQueue(PlayerKey playerId, QueueKey queueGroupId, QueueUnit<TCellKey> unit) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.queues.TryGetValue(queueGroupId, out IUnitQueue<TCellKey>? queue)) {
			return Error.NotFound();
		}
		return queue.addUnit(unit);
	}

	public ErrorOr<Success> deleteUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit_id) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.queues.TryGetValue(queueGroupId, out IUnitQueue<TCellKey>? queue)) {
			return Error.NotFound();
		}
		ErrorOr<QueueUnit<TCellKey>> unit = queue.removeUnit(unit_id);
		if (unit.IsError) {
			return Error.NotFound();
		}
		return Result.Success;
	}

	public ErrorOr<Success> deployUnitFromQueue(PlayerKey playerId, QueueKey queueGroupId, UnitKey unit_id, TCellKey pos) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.queues.TryGetValue(queueGroupId, out IUnitQueue<TCellKey>? queue)) {
			return Error.NotFound();
		}

		// Remove from queue
		ErrorOr<QueueUnit<TCellKey>> unit = queue.removeUnit(unit_id);
		if (unit.IsError) {
			return Error.NotFound();
		}

		// Deploy on the map
		MapUnit<TCellKey> map_unit = unit.Value.deploy(pos);
		player.units[unit_id] = map_unit;
		return Result.Success;
	}



	// Unit
	public ErrorOr<MapUnit<TCellKey>> getUnit(PlayerKey playerId, UnitKey unitId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.units.TryGetValue(unitId, out var unit)) {
			return Error.NotFound();
		}
		return unit.ToErrorOr();
	}

	public ErrorOr<MapUnit<TCellKey>[]> getAllUnitsVisibleFromPlayer(PlayerKey playerId) {
		return players.Values.SelectMany(p => p.units.Values).ToArray().ToErrorOr();
	}

	public ErrorOr<MapUnit<TCellKey>[]> getAllUnitsOfPlayer(PlayerKey playerId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		return player.units.Values.ToArray().ToErrorOr();
	}

	public ErrorOr<Success> moveUnit(PlayerKey playerId, UnitKey unitId, TCellKey cellId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.units.TryGetValue(unitId, out var unit)) {
			return Error.NotFound();
		}
		unit.position = cellId;
		return Result.Success;
	}

	public ErrorOr<Success> deleteUnit(PlayerKey playerId, UnitKey unitId) {
		if (!players.TryGetValue(playerId, out var player)) {
			return Error.NotFound();
		}
		if (!player.units.Remove(unitId)) {
			return Error.NotFound();
		}
		return Result.Success;
	}



	// Combat
	public ErrorOr<ICombat<TCellKey>> getCombatInfo(PlayerKey playerId, uint combatId) {
		throw new NotImplementedException();
	}
}
