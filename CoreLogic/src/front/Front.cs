namespace CoreLogic;

internal class Front : IFront {
	public uint id => throw new NotImplementedException();

	public IEnumerable<(uint cellId1, uint cellId2)> edges => throw new NotImplementedException();

	public IEnumerable<(uint cellId1, uint cellId2)> extremities => throw new NotImplementedException();
}
