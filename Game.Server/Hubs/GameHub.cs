using Microsoft.AspNetCore.SignalR;
using Game.Server.Singletons;
using Game.Core;

namespace Game.Server.Hubs;

internal class GameHub(GameManager gameManager) : Hub {

	private readonly GameManager _gameManager = gameManager;

	public async Task CreateGameAsync() {
		string gameId = _gameManager.CreateGame();
		await Groups.AddToGroupAsync(Context.ConnectionId, gameId).ConfigureAwait(true);
		Console.WriteLine("Game created: " + gameId);
	}


	public async Task<bool> JoinGameAsync(string gameId) {
		// if (!_gameManager.TryGetGame(gameId, out GameRoot? game)) return false;

		await Groups.AddToGroupAsync(Context.ConnectionId, gameId).ConfigureAwait(true);

		// await Clients.Group(code).SendAsync("PlayerJoined", Context.ConnectionId);

		await Clients.Group(gameId).SendAsync("JoinedAsync", Context.ConnectionId).ConfigureAwait(true);
		await Clients.Group(gameId).SendAsync("JoinedAsyncTest", Context.ConnectionId).ConfigureAwait(true);

		Console.WriteLine("Game joined: " + gameId + " user: " + Context.ConnectionId);

		return true;
	}



	/*
	public async Task Send(string message) => await Clients.All.SendAsync("Receive", message).ConfigureAwait(true);


	private string GenerateLobbyCode() {
		string lobbyCode;
		do {
			lobbyCode = Guid.NewGuid().ToString();
		} while (_lobbies.ContainsKey(lobbyCode));
		return lobbyCode;
	}
	*/
}
