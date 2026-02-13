using CoreLogic.Units;

namespace CoreLogic;

public class GameCell : Cell {
	public int ownership { get; }
	public IList<BaseUnit> units { get; } = [];
}
