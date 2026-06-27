namespace CoreLogic;

internal class UnitQueue : IUnitQueue {
	public uint id => throw new NotImplementedException();

	public uint parallelUnitPoints => throw new NotImplementedException();

	public IEnumerable<(IUnit res, uint progress)> units => throw new NotImplementedException();
}
