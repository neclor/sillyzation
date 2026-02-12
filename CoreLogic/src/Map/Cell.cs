namespace Game;

public enum Terrain {
	Plain
}

public enum Ressource {
	Oil
}

public class Cell {
	public Terrain Terrain { get; }
	public uint Population { get; }
	public List<(Ressource, int)> Ressources { get; } = [];
}