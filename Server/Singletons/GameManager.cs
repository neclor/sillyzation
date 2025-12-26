using System.Collections.Concurrent;
using Server;
using Core;

namespace Server.Singletons;

internal class GameManager {

	public IReadOnlyDictionary<string, Game> Games => _games;
	public readonly ConcurrentDictionary<string, Game> _games = [];

	public (string GameCode, Guid HostId) CreateGame() {
		Game game = new();
		string gameCode = NewGameCode();

		_games[gameCode] = game;

		return (gameCode, game.Lobby.Host.Id);
	}

	private static string NewGameCode() => Guid.NewGuid().ToString()[..8];
}
