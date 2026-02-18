using CoreLogic.Units;

namespace CoreLogic.Map;

public class GameCell : Cell {
	public int ownership { get; }
	public IList<BaseUnit> units { get; } = [];
}
