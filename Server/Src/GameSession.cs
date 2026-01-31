using System.Collections.Concurrent;
using Core;

namespace Server;

internal class GameSession {

	public string GameId { get; } = Guid.NewGuid().ToString()[..8];
	public ConcurrentDictionary<string, Guid> PlayerIdMap { get; } = [];
	public Game Game { get; } = new();

	public bool TryAddPlayer(string userId, out Guid playerId) {




	}



}
