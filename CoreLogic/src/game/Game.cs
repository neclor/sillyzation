using ErrorOr;

namespace CoreLogic;

internal class Game : IGame {
	private Dictionary<uint, IPlayer> players = [];
	private PlayerKey playerId = 1;



	public Game(IEnumerable<(string name, Color color)> players) {
		foreach (var player in players) {
			var status = addPlayer(player.name, player.color);
			if (status.IsError) {
				throw new InvalidDataException("Failed to insert players");
			}
		}
	}




	public ErrorOr<IPlayer> getPlayer(PlayerKey playerId) {
		IPlayer? player = players[playerId];
		if (player == null) {
			return Error.NotFound();
		}
		return player.ToErrorOr();
	}



	public Dictionary<uint, IPlayer> getAllPlayers() {
		return players;
	}



	public ErrorOr<bool> addPlayer(string name, Color color) {
		try {
			playerId = playerId++;
			players[playerId] = new Player(
				playerId,
				name,
				color
			);
			return true;
		}
		catch (ArgumentNullException) {
			return false;
		}
	}



	public ErrorOr<bool> kickPlayer(PlayerKey playerId) {
		try {
			_ = players.Remove(playerId);
		}
		catch (ArgumentNullException) {
			return Error.NotFound("Player to remove not found");
		}
		return true;
	}
}