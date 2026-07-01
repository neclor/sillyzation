namespace CoreLogic;

public interface IUnit<TCellKey> {
	uint id { get; }
	string name { get; }
	uint baseHealth { get; }
	uint health { get; }
	uint speed { get; }
	uint owner { get; }
	TCellKey position { get; }
};
