namespace CoreLogic;

internal class Combat<TCellKey> : ICombat<TCellKey> {
	public uint id => throw new NotImplementedException();

	public IEnumerable<IUnit<TCellKey>> defenderUnit => throw new NotImplementedException();

	public IEnumerable<IUnit<TCellKey>> attackerUnit => throw new NotImplementedException();

	public int combatStatus => throw new NotImplementedException();
}
