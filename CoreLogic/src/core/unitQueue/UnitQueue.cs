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
		try {
			QueueUnit<TCellKey> elem = units.FirstOrDefault(e => e.id == unit_id);
			// if (elem.progress != 100) {
			// 	return Error.Failure("Unit is not ready");
			// }
			if (!units.Remove(elem)) {
				return Error.NotFound();
			}
			return Result.Success;
		}
		catch (ArgumentNullException) {
			return Error.NotFound();
		}
	}
}
