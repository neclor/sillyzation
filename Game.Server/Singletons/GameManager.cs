using System.Collections.Concurrent;
using Game.Core;

namespace Game.Server.Singletons;

internal class GameManager {

	public IReadOnlyDictionary<string, GameRoot> Games => _games;

	public readonly ConcurrentDictionary<string, GameRoot> _games = [];

	public string CreateGame() {
		string gameId = NewGameId();
		_games[gameId] = new GameRoot();
		return gameId;
	}

	// public bool TryRemoveGame(string code, [MaybeNullWhen(false)] out GameRoot game) => _games.TryRemove(code, out game);

	private static string NewGameId() => Guid.NewGuid().ToString()[..6];
}
