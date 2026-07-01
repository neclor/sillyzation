namespace CoreLogic;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IUnitQueue<TCellKey> {
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
	uint id { get; }
	uint parallelUnitPoints { get; }
	IEnumerable<(IUnit<TCellKey> res, uint progress)> units { get; }
};
