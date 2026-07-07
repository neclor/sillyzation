using ErrorOr;

namespace CoreLogic;

internal class Game : IGame {
	private readonly Dictionary<PlayerKey, IPlayer> players;
	private PlayerKey playerId = 1;



	public Game(IEnumerable<IPlayer> players) => this.players = players.ToDictionary(p => p.id);




	public ErrorOr<IPlayer> getPlayer(PlayerKey playerId) {
		IPlayer? player = players[playerId];
		if (player == null) {
			return Error.NotFound();
		}
		return player.ToErrorOr();
	}



	public Dictionary<PlayerKey, IPlayer> getAllPlayers() {
		return players;
	}



	public ErrorOr<Success> addPlayer(string name, Color color) {
		try {
			players[playerId] = new Player(
				playerId,
				name,
				color
			);
			playerId++;
			return Result.Success;
		}
		catch (ArgumentNullException) {
			return Error.NotFound();
		}
	}



	public ErrorOr<Success> kickPlayer(PlayerKey playerId) {
		try {
			_ = players.Remove(playerId);
		}
		catch (ArgumentNullException) {
			return Error.NotFound("Player to remove not found");
		}
		return Result.Success;
	}
}