namespace CoreLogic;

public interface IFront {
	uint id { get; }
	IEnumerable<(uint cellId1, uint cellId2)> edges { get; }
	IEnumerable<(uint cellId1, uint cellId2)> extremities { get; }
};
