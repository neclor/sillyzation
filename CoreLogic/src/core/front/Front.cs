namespace CoreLogic;

internal class Front<TCellKey> : IFront<TCellKey> {
	public uint id => throw new NotImplementedException();

	public IEnumerable<(TCellKey cellId1, TCellKey cellId2)> edges => throw new NotImplementedException();
	public IEnumerable<(TCellKey cellId1, TCellKey cellId2)> extremities => throw new NotImplementedException();
}
