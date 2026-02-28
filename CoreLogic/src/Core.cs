

using ErrorOr;

namespace CoreLogic;


internal class Core {

	private readonly Map map;
	private IEnumerable<IPlayer> players = [];
	private PlayerKey playerId = 1;

	public Core(
		IEnumerable<PlayerInit> players,
		IEnumerable<(CellKey key, ICell cell)> cells,
		IEnumerable<(CellKey key1, CellKey key2)> connexions
	) {
		// Players
		foreach (var player in players) {
			var status = addPlayer(player.name, player.color);
			if (status.IsError) {
				throw new InvalidDataException("Failed to insert players");
			}
		}

		// Map
		map = new(cells, connexions);
	}

	public ErrorOr<IGame> nextGameTick() {
		return new Game();
	}

	public ErrorOr<IGame> syncGame() {
		return new Game();
	}



	// Player
	public ErrorOr<IPlayer> getPlayer(PlayerKey playerId) {
		IPlayer? player = players.First(player => player.id == playerId);
		if (player == null) {
			return Error.NotFound();
		}
		return player.ToErrorOr();
	}

	public IEnumerable<IPlayer> getAllPlayers() {
		return players;
	}

	public ErrorOr<bool> addPlayer(string name, Color color) {
		try {
			players = players.Append(new Player(
				playerId++,
				name,
				color
			));
			return true;
		}
		catch (ArgumentNullException) {
			return false;
		}
	}

	public ErrorOr<bool> kickPlayer(PlayerKey playerId) {
		try {
			players = players
				.Where(player => player.id != playerId)
				.ToList();
		}
		catch (ArgumentNullException) {
			return Error.NotFound("Player to remove not found");
		}
		return true;
	}



	// Cells
	public ErrorOr<ICell> getCell(PlayerKey playerId, CellKey cellId) {
		// TODO Add player protection
		return map.getCell(cellId);
	}



	// Unit Queue
	public ErrorOr<IUnitQueue> getUnitQueue(PlayerKey playerId) {
		return new UnitQueue();
	}

	public ErrorOr<bool> createUnitQueueGroup(PlayerKey playerId) {
		return true;
	}

	public ErrorOr<bool> deployUnitQueueGroup(PlayerKey playerId, uint queueGroupId) {
		return true;
	}

	public ErrorOr<bool> addUnitToQueueGroup(PlayerKey playerId) {
		return true;
	}

	public ErrorOr<bool> removeUnitToQueueGroup(PlayerKey playerId, uint unitInQueueGroupId) {
		return true;
	}



	// Unit
	public ErrorOr<IUnit> getUnit(PlayerKey playerId, uint unitId) {
		return new Unit();
	}

	public ErrorOr<IEnumerable<IUnit>> getAllUnits(PlayerKey playerId) {
		return new[] { new Unit() };
	}

	public ErrorOr<bool> moveUnit(PlayerKey playerId, uint unitId, CellKey cellId) {
		return true;
	}

	public ErrorOr<bool> assignUnitToFront(PlayerKey playerId, uint unitId, uint frontId) {
		return true;
	}

	public ErrorOr<bool> deleteUnit(PlayerKey playerId, uint unitId) {
		return true;
	}



	// Combat
	public ErrorOr<ICombat> getCombatInfo(PlayerKey playerId, uint combatId) {
		return new Combat();
	}



	// Front
	public ErrorOr<IFront> getFront(PlayerKey playerId, uint frontId) {
		return new Front();
	}

	public ErrorOr<bool> createFront(PlayerKey playerId, CellKey cellId1, CellKey cellId2) {
		return true;
	}

	public ErrorOr<bool> moveFront(PlayerKey playerId, uint frontId, CellKey cellId, bool side) {
		return true;
	}
}