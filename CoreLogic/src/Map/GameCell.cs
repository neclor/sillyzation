namespace Game;

public class GameCell : Cell {
	public int Ownership { get; }
	public List<BaseUnit> units { get; }

	public void AddUnit(BaseUnit unit) => units.Add(unit);
}
