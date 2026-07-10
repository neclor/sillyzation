using ErrorOr;

namespace CoreLogic;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IUnitQueue<TCellKey> where TCellKey : notnull {
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
	uint id { get; }
	(Unit<TCellKey> unit, uint progress)[] getUnits();
	ErrorOr<Success> addUnit(Unit<TCellKey> unit);
	ErrorOr<Success> removeUnit(UnitKey unitId);
};
