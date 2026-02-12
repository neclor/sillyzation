using Game.CoreLogic.Units;

namespace Game.CoreLogic;

public class GameCell : Cell {

	public int Ownership { get; }
	public IList<BaseUnit> Units { get; } = [];

	public void AddUnit(BaseUnit unit) => Units.Add(unit);
}
