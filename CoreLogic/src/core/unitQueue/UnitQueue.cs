using ErrorOr;

namespace CoreLogic;

internal class UnitQueue<TCellKey> : IUnitQueue<TCellKey> where TCellKey : notnull {
	private static uint id_counter = 1;
	public uint id { get; }
	public List<QueueUnit<TCellKey>> units { get; }

	public UnitQueue() {
		id = id_counter++;
		units = [];
	}

	public QueueUnit<TCellKey>[] getUnits() {
		foreach (QueueUnit<TCellKey> unit in units) {
			Console.WriteLine(unit.id + " " + unit.progress);
		}
		return [.. units];
	}

	public ErrorOr<Success> addUnit(QueueUnit<TCellKey> unit) {
		units.Add(unit);
		return Result.Success;
	}

	public ErrorOr<Success> removeUnit(UnitKey unit_id) {
		QueueUnit<TCellKey>? elem = units.FirstOrDefault(e => e!.id == unit_id, default);
		if (elem == null) {
			return Error.NotFound();
		}
		if (!elem.ready) {
			return Error.Failure("Unit is not ready");
		}
		if (!units.Remove(elem)) {
			return Error.NotFound();
		}
		return Result.Success;
	}

	public ErrorOr<Success> tick() {
		foreach (QueueUnit<TCellKey> unit in units) {
			unit.tick();
		}
		return Result.Success;
	}
}
