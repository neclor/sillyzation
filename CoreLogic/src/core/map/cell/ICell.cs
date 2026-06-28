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

public interface ICell<TCellKey> where TCellKey : notnull {
	TCellKey id { get; }
	string name { get; }
	PlayerKey? owner { get; set; }
	Terrain terrain { get; }
	IEnumerable<(Ressource res, uint amount)> ressources { get; }
};