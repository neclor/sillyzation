namespace CoreLogic.Map;


internal class Cell<TKey> : ICell<TKey>
	where TKey : notnull, IEquatable<TKey> {

	public TKey id { get; }
	public string name { get; }
	public uint? owner { get; }
	public Terrain terrain { get; }
	public IEnumerable<(Ressource res, uint amount)> ressources { get; }

	public Cell(
		TKey id,
		string name,
		Terrain terrain,
		IEnumerable<(Ressource res, uint amount)> ressources
	) {
		this.id = id;
		this.name = name;
		this.terrain = terrain;
		this.ressources = ressources?.ToList() ?? [];
	}
}

