using CoreLogic.Units;

namespace CoreLogic;

public class GameCell : Cell {
	public int Ownership { get; }
	public IList<BaseUnit> Units { get; } = [];
}
