using ErrorOr;

namespace CoreLogic;

public record Game();

public record Player(
	uint id,
	string name
);

public enum Ressource {
	Oil,
}

public enum Terrain {
	Plain,
}

public record Cell(
	uint id,
	string name,
	uint? owner,
	Terrain terrain,
	IEnumerable<(Ressource res, uint amount)> ressources
);

public record Unit(
	uint id,
	uint baseHealth,
	uint health,
	uint speed,
	uint owner,
	uint position
);

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public record UnitQueue(
	uint id,
	uint parallelUnitPoints,
	IEnumerable<(Unit res, uint progress)> units
);
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix

public record Combat(
	uint id,
	IEnumerable<Unit> defenderUnit,
	IEnumerable<Unit> attackerUnit,

	/**
	Number from -100 to 100
	where 100 is the attacker winning
	and -100 is the defender winning
	*/
	int combatStatus
);

public record Front(
	uint id,
	IEnumerable<(uint cellId1, uint cellId2)> edges,
	IEnumerable<(uint cellId1, uint cellId2)> extremities
);

public interface ICore {

	// Game
	ErrorOr<Game> nextGameTick();
	ErrorOr<Game> syncGame();

	// Player
	ErrorOr<Player> getPlayer(uint playerId);
	ErrorOr<IEnumerable<Player>> getAllPlayers();
	ErrorOr<bool> addPlayer();
	ErrorOr<bool> kickPlayer();

	// Cells
	ErrorOr<Cell> getCell(uint playerId, uint cellId);

	// Unit Queue
	ErrorOr<UnitQueue> getUnitQueue(uint playerId);
	ErrorOr<bool> createUnitQueueGroup(uint playerId);
	ErrorOr<bool> deployUnitQueueGroup(uint playerId, uint queueGroupId);
	ErrorOr<bool> addUnitToQueueGroup(uint playerId);
	ErrorOr<bool> removeUnitToQueueGroup(uint playerId, uint unitInQueueGroupId);

	// Unit
	ErrorOr<Unit> getUnit(uint playerId, uint unitId);
	ErrorOr<IEnumerable<Unit>> getAllUnits(uint playerId);
	ErrorOr<bool> moveUnit(uint playerId, uint unitId, uint cellId);
	ErrorOr<bool> assignUnitToFront(uint playerId, uint unitId, uint frontId);
	ErrorOr<bool> deleteUnit(uint playerId, uint unitId);

	// Combat
	ErrorOr<Combat> getCombatInfo(uint playerId, uint combatId);

	// Front
	ErrorOr<Front> getFront(uint playerId, uint frontId);
	ErrorOr<bool> createFront(uint playerId, uint cellId1, uint cellId2);
	ErrorOr<bool> moveFront(uint playerId, uint frontId, uint cellId, bool side);
}