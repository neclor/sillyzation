namespace CoreLogic;

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
