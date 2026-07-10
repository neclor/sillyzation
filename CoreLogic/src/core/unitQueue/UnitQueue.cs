using ErrorOr;

namespace CoreLogic;

internal class UnitQueue<TCellKey> : IUnitQueue<TCellKey> where TCellKey : notnull {
	private static uint id_counter = 1;
	public uint id { get; }
	public List<(Unit<TCellKey> unit, uint progress)> units { get; }

	public UnitQueue() {
		id = id_counter++;
		units = [];
	}

	public (Unit<TCellKey> unit, uint progress)[] getUnits() {
		foreach ((Unit<TCellKey> unit, uint progress) in units) {
			Console.WriteLine(unit.id + " " + progress);
		}
		return [.. units];
	}

	public ErrorOr<Success> addUnit(Unit<TCellKey> unit) {
		units.Add((unit, 0));
		return Result.Success;
	}

	public ErrorOr<Success> removeUnit(UnitKey unit_id) {
		try {
			(Unit<TCellKey> unit, uint progress) elem = units.FirstOrDefault(e => e.unit.id == unit_id);
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
