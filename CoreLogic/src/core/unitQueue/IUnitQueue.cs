namespace CoreLogic;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IUnitQueue {
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
	uint id { get; }
	uint parallelUnitPoints { get; }
	IEnumerable<(IUnit res, uint progress)> units { get; }
};
