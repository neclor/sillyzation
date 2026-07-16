using ErrorOr;

namespace CoreLogic;

internal class UnitQueue<TCellKey> : IUnitQueue<TCellKey> where TCellKey : notnull {
	private static uint id_counter = 1;
	public uint id { get; }
	public Dictionary<UnitKey, QueueUnit<TCellKey>> units { get; }

	public UnitQueue() {
		id = id_counter++;
		units = [];
	}

	public QueueUnit<TCellKey>[] getUnits() {
		foreach (QueueUnit<TCellKey> unit in units.Values) {
			Console.WriteLine(unit.id + " " + unit.progress);
		}
		return [.. units.Values];
	}

	public ErrorOr<Success> addUnit(QueueUnit<TCellKey> unit) {
		units[unit.id] = unit;
		return Result.Success;
	}

	public ErrorOr<QueueUnit<TCellKey>> removeUnit(UnitKey unit_id) {
		if (!units.TryGetValue(unit_id, out QueueUnit<TCellKey>? unit)) {
			return Error.NotFound();
		}
		if (!unit.ready) {
			return Error.Failure("Unit is not ready");
		}
		if (!units.Remove(unit_id)) {
			return Error.NotFound();
		}
		return unit;
	}

	public ErrorOr<Success> tick() {
		foreach (QueueUnit<TCellKey> unit in units.Values) {
			unit.tick();
		}
		return Result.Success;
	}
}
