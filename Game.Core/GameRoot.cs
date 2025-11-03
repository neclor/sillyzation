using Game.Core.World;

namespace Game.Core;

public class GameRoot {

	public PlayerManager PlayerManger { get; } = new();

	public WorldManager WorldManager { get; } = new();

	public bool TryStartGame() {
		if (PlayerManger.Players.Count < 2) return false;

		foreach (Player player in PlayerManger.Players) {
			if (!player.IsReady) return false;
		}

		WorldManager.InitializeWorld(PlayerManger.Players);
		return true;
	}
}
