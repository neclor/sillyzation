namespace CoreLogic;

public enum Ressource {
	Oil,
}

public enum Terrain {
	Plain,
}

public class Cell {
	public Terrain terrain { get; }
	public uint population { get; }
	public IList<(Ressource ressource, int amount)> ressources { get; } = [];
}
