namespace CoreLogic;

internal class UnitQueue<TCellKey> : IUnitQueue<TCellKey> {
	public uint id => throw new NotImplementedException();

	public uint parallelUnitPoints => throw new NotImplementedException();

	public IEnumerable<(IUnit<TCellKey> res, uint progress)> units => throw new NotImplementedException();
}
