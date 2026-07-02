namespace CoreLogic;

public interface IUnit<TCellKey> {
	UnitKey id { get; }
	string name { get; }
	uint baseHealth { get; }
	uint health { get; }
	uint speed { get; }
	PlayerKey owner { get; }
	TCellKey? position { get; }
};
