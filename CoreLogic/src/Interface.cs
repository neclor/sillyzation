using ErrorOr;

namespace CoreLogic;

public interface IGame {
	IEnumerable<IPlayer> players { get; }
};

public interface IPlayer {
	uint id { get; }
	string name { get; }
};

public enum Ressource {
	Oil,
}

public enum Terrain {
	Plain,
}

public interface ICell {
	uint id { get; }
	string name { get; }
	uint? owner { get; }
	Terrain terrain { get; }
	IEnumerable<(Ressource res, uint amount)> ressources { get; }
};

public interface IUnit {
	uint id { get; }
	uint baseHealth { get; }
	uint health { get; }
	uint speed { get; }
	uint owner { get; }
	uint position { get; }
};

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IUnitQueue {
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
	uint id { get; }
	uint parallelUnitPoints { get; }
	IEnumerable<(IUnit res, uint progress)> units { get; }
};

public interface ICombat {
	uint id { get; }
	IEnumerable<IUnit> defenderUnit { get; }
	IEnumerable<IUnit> attackerUnit { get; }

	/**
	Number from -100 to 100
	where 100 is the attacker winning
	and -100 is the defender winning
	*/
	int combatStatus { get; }
};

public interface IFront {
	uint id { get; }
	IEnumerable<(uint cellId1, uint cellId2)> edges { get; }
	IEnumerable<(uint cellId1, uint cellId2)> extremities { get; }
};

public interface ICore {

	// Game
	ErrorOr<IGame> nextGameTick();
	ErrorOr<IGame> syncGame();

	// Player
	ErrorOr<IPlayer> getPlayer(uint playerId);
	ErrorOr<IEnumerable<IPlayer>> getAllPlayers();
	ErrorOr<bool> addPlayer();
	ErrorOr<bool> kickPlayer();

	// Cells
	ErrorOr<ICell> getCell(uint playerId, uint cellId);

	// Unit Queue
	ErrorOr<IUnitQueue> getUnitQueue(uint playerId);
	ErrorOr<bool> createUnitQueueGroup(uint playerId);
	ErrorOr<bool> deployUnitQueueGroup(uint playerId, uint queueGroupId);
	ErrorOr<bool> addUnitToQueueGroup(uint playerId);
	ErrorOr<bool> removeUnitToQueueGroup(uint playerId, uint unitInQueueGroupId);

	// Unit
	ErrorOr<IUnit> getUnit(uint playerId, uint unitId);
	ErrorOr<IEnumerable<IUnit>> getAllUnits(uint playerId);
	ErrorOr<bool> moveUnit(uint playerId, uint unitId, uint cellId);
	ErrorOr<bool> assignUnitToFront(uint playerId, uint unitId, uint frontId);
	ErrorOr<bool> deleteUnit(uint playerId, uint unitId);

	// Combat
	ErrorOr<ICombat> getCombatInfo(uint playerId, uint combatId);

	// Front
	ErrorOr<IFront> getFront(uint playerId, uint frontId);
	ErrorOr<bool> createFront(uint playerId, uint cellId1, uint cellId2);
	ErrorOr<bool> moveFront(uint playerId, uint frontId, uint cellId, bool side);
}
