using ErrorOr;

namespace CoreLogic;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IUnitQueue<TCellKey> where TCellKey : notnull {
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
	uint id { get; }
	QueueUnit<TCellKey>[] getUnits();
	ErrorOr<Success> addUnit(QueueUnit<TCellKey> unit);
	ErrorOr<QueueUnit<TCellKey>> removeUnit(UnitKey unitId);
	ErrorOr<Success> tick();
};
