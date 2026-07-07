using ErrorOr;

namespace CoreLogic;

internal class UnitQueue<TCellKey> : IUnitQueue<TCellKey> {
	private static uint id_counter = 1;
	public uint id { get; }
	public List<(IUnit<TCellKey> unit, uint progress)> units { get; }

	IEnumerable<(IUnit<TCellKey> unit, uint progress)> IUnitQueue<TCellKey>.units => units;

	public UnitQueue() {
		id = id_counter++;
		units = [];
	}

	public ErrorOr<Success> addUnit(IUnit<TCellKey> unit) {
		units.Add((unit, 0));
		return Result.Success;
	}

	public ErrorOr<Success> removeUnit(UnitKey unit_id) {
		try {
			(IUnit<TCellKey> unit, uint progress) elem = units.FirstOrDefault(e => e.unit.id == unit_id);
			if (elem.progress != 100) {
				return new Error();
			}
			if (!units.Remove(elem)) {
				return new Error();
			}
			return Result.Success;
		}
		catch (ArgumentNullException) {
			return new Error();
		}
	}
}
