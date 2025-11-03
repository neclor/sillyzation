using Game.Core.Common.Grid;

namespace Game.Core.World;

public class WorldManager {

	private Grid<GridCell>? _worldGrid;

	public void InitializeWorld(IReadOnlyList<Player> players) {
		_worldGrid = new Grid<GridCell>(100, 100, () => new());

		foreach (Player player in players) {

		}
	}
}
