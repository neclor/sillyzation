namespace CoreLogic.Map;


internal class Cell : ICell {
	public uint id => throw new NotImplementedException();

	public string name => throw new NotImplementedException();

	public uint? owner => throw new NotImplementedException();

	public Terrain terrain => throw new NotImplementedException();

	public IEnumerable<(Ressource res, uint amount)> ressources => throw new NotImplementedException();
}
