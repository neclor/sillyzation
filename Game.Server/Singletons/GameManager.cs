using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Game.Core;

namespace Game.Server.Singletons;

internal class GameManager {

	private readonly ConcurrentDictionary<string, GameRoot> _games = [];

	public string CreateGame() {
		string gameId = NewGameId();
		_games[gameId] = new GameRoot();
		return gameId;
	}

	public bool TryGetGame(string gameId, [MaybeNullWhen(false)] out GameRoot game) => _games.TryGetValue(gameId, out game);


	public bool TryAddPlayer(string gameId, )



	// public bool TryRemoveGame(string code, [MaybeNullWhen(false)] out GameRoot game) => _games.TryRemove(code, out game);


	private static string NewGameId() => Guid.NewGuid().ToString()[..6];
}
