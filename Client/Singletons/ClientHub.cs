using Shared.Interfaces;

namespace Client.Singletons;

internal class ClientHub : IClientHub {

	public async Task UpdateLobbyAsync() => Console.WriteLine("Lobby updated from server");


}
