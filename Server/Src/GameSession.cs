using System.Collections.Concurrent;
using Core;

namespace Server;

internal class GameSession {

	public Game Game { get; } = new();



	public Guid? TryCreatePlayer() {
		Player player = new();
		if (!Game.Lobby.TryAddPlayer(player)) return null;

		return player.Id;
	}




}
