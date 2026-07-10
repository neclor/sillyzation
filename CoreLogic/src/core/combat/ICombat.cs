namespace CoreLogic;

public interface ICombat<TCellKey> where TCellKey : notnull {
	uint id { get; }
	IEnumerable<Unit<TCellKey>> defenderUnit { get; }
	IEnumerable<Unit<TCellKey>> attackerUnit { get; }

	/**
	Number from -100 to 100
	where 100 is the attacker winning
	and -100 is the defender winning
	*/
	int combatStatus { get; }
};
