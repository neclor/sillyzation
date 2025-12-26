using Microsoft.AspNetCore.SignalR;
using Server.Singletons;
using Shared.Interfaces;
using Core;

namespace Server.Hubs;

internal class GameHub(GameManager gameManager) : Hub<IClientHub>, IServerHub {

	private readonly GameManager _gameManager = gameManager;

	public async Task<(string GameCode, Guid Id)> CreateGameAsync() => _gameManager.CreateGame();

	public async Task<string?> JoinGameAsync(string gameCode) {
		if (!_gameManager.Games.TryGetValue(gameCode, out Game? game)) return null;


		Player





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
