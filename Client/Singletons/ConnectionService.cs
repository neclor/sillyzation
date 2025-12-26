using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Interfaces;
using Client.SignalRClientSourceGenerator;

namespace Client.Singletons;

internal class ConnectionService : IClientHub {

	public IServerHub Connection { get; }

	public ConnectionService(NavigationManager nav) {
		HubConnection hubConnection = new HubConnectionBuilder()
			.WithUrl(nav.ToAbsoluteUri("/lobby"))
			.WithAutomaticReconnect()
			.Build();

		Connection = hubConnection.ServerProxy<IServerHub>();
		_ = hubConnection.ClientRegistration<IClientHub>(this);
	}

	public async Task UpdateLobbyAsync() => Console.WriteLine("Lobby updated from server");
}
