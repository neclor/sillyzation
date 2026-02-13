namespace CoreLogic;

public enum Ressource {
	Oil,
}

public enum Terrain {
	Plain,
}

public class Cell {
	public Terrain Terrain { get; }
	public uint Population { get; }
	public IList<(Ressource Ressource, int Amount)> Ressources { get; } = [];
}
