using Microsoft.AspNetCore.SignalR;
using Game.Server.Singletons;
using Game.Core;

namespace Game.Server.Hubs;

internal class GameHub(GameManager gameManager) : Hub {

	private readonly GameManager _gameManager = gameManager;

	public async Task<string> CreateGame() {
		string gameId = _gameManager.CreateGame();
		await Groups.AddToGroupAsync(Context.ConnectionId, gameId).ConfigureAwait(true);
		return gameId;
	}

	public async Task<bool> JoinGame(string gameId) {
		if (!_gameManager.TryGetGame(gameId, out GameRoot? game)) return false;

		await Groups.AddToGroupAsync(Context.ConnectionId, gameId).ConfigureAwait(true);

		await Clients.Group(code).SendAsync("PlayerJoined", Context.ConnectionId);





		return true;
	}




	public async Task Send(string message) => await Clients.All.SendAsync("Receive", message).ConfigureAwait(true);


	private string GenerateLobbyCode() {
		string lobbyCode;
		do {
			lobbyCode = Guid.NewGuid().ToString();
		} while (_lobbies.ContainsKey(lobbyCode));
		return lobbyCode;
	}
}
