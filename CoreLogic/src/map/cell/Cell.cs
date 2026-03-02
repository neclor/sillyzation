namespace CoreLogic;

internal class Cell : ICell {

	public CellKey id { get; }
	public string name { get; }
	public PlayerKey? owner { get; }
	public Terrain terrain { get; }
	public uint population { get; }
	public IEnumerable<(Ressource res, uint amount)> ressources { get; }
	private IList<PlayerKey> isKnownByList { get; set; } = [];

	public Cell(
		CellKey id,
		string name,
		Terrain terrain,
		uint population,
		IEnumerable<(Ressource res, uint amount)> ressources
	) {
		this.id = id;
		this.name = name;
		this.terrain = terrain;
		this.population = population;
		this.ressources = ressources?.ToList() ?? [];
	}

	public void explore(PlayerKey playerId) {
		isKnownByList.Add(playerId);
	}

	public bool isKnownBy(PlayerKey playerId) {
		return isKnownByList.Any(player => player == playerId);
	}
}

