namespace CoreLogic;

public interface IUnit {
	uint id { get; }
	uint baseHealth { get; }
	uint health { get; }
	uint speed { get; }
	uint owner { get; }
	uint position { get; }
};
