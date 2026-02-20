namespace CoreLogic;

public enum Ressource {
	Oil,
}

public enum Terrain {
	Plain,
}

public interface ICell {
	uint id { get; }
	string name { get; }
	uint? owner { get; }
	Terrain terrain { get; }
	IEnumerable<(Ressource res, uint amount)> ressources { get; }
};