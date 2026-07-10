namespace CoreLogic;

internal class Combat<TCellKey> : ICombat<TCellKey> where TCellKey : notnull {
	public uint id => throw new NotImplementedException();

	public IEnumerable<Unit<TCellKey>> defenderUnit => throw new NotImplementedException();

	public IEnumerable<Unit<TCellKey>> attackerUnit => throw new NotImplementedException();

	public int combatStatus => throw new NotImplementedException();
}
