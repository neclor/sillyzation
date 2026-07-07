using ErrorOr;

namespace CoreLogic;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IUnitQueue<TCellKey> {
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
	uint id { get; }
	(IUnit<TCellKey> unit, uint progress)[] getUnits();
	ErrorOr<Success> addUnit(IUnit<TCellKey> unit);
	ErrorOr<Success> removeUnit(UnitKey unitId);
};
