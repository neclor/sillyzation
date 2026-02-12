using QuikGraph;

namespace Game;

public enum Terrain {
	Plain
}

public enum Ressource {
	Oil
}

public enum UnitType {
	Tank,
	Infantry
}

public class BaseUnit {
	public uint BaseHealth { get; }
	public uint Health { get; }
	public uint Speed { get; }

	public BaseUnit(
		uint BaseHealth,
		uint Health,
		uint Speed
	) {
		this.BaseHealth = BaseHealth;
		this.Health = Health;
		this.Speed = Speed;
	}
}

public class Tank : BaseUnit {
	public Tank() : base(50, 10, 5) { }
}

public class Cell {
	public Terrain Terrain { get; }
	public uint Population { get; }
	public List<(Ressource, int)> Ressources { get; }
}

public class GameCell : Cell {
	public int Ownership { get; }
	public List<BaseUnit> units { get; }

	public void AddUnit(BaseUnit unit) {
		units.Add(unit);
	}
}

public class Map {
	private UndirectedGraph<string, Edge<string>> map = new();
}

public class CoreLogic {
	public static UndirectedGraph<string, Edge<string>> getGraph() {
		GameCell cell = new();

		cell.AddUnit(new Tank());

		return new UndirectedGraph<string, Edge<string>>();
	}
}
