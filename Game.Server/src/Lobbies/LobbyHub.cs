using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace Game.Server.Lobbies;

internal class LobbyHub : Hub {

	private readonly ConcurrentDictionary<string, Lobby> _lobbies = new();

	public async Task<string> CreateLobby() {
		string lobbyCode = GenerateLobbyCode();

		Lobby lobby = new() { Code = lobbyCode, OwnerId = Context.ConnectionId };

		_lobbies[lobbyCode] = lobby;

		await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode).ConfigureAwait(true);
		return lobbyCode;
	}

	public async Task<bool> JoinLobby(string lobbyCode) {
		if (!_lobbies.TryGetValue(lobbyCode, out Lobby? lobby)) return false;

		lobby.AddPlayer(Context.ConnectionId);
		await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode).ConfigureAwait(true);
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
