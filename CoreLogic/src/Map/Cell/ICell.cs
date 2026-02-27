namespace CoreLogic;

public enum Ressource {
	Oil,
	Steel,
}

public enum Terrain {
	Plain,
	Forest,
	Desert,
	Tundra,
	Swamp,
	Savanna,
	Jungle,
}

public interface ICell<TKey> {
	TKey id { get; }
	string name { get; }
	uint? owner { get; }
	Terrain terrain { get; }
	IEnumerable<(Ressource res, uint amount)> ressources { get; }
};