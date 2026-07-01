namespace CoreLogic;

public interface IFront<TCellKey> {
	uint id { get; }
	IEnumerable<(TCellKey cellId1, TCellKey cellId2)> edges { get; }
	IEnumerable<(TCellKey cellId1, TCellKey cellId2)> extremities { get; }
};
