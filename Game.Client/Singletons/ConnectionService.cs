using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Game.Client.Singletons;

internal class ConnectionService {

	public HubConnection HubConnection { get; }

	public ConnectionService(NavigationManager nav) {


		HubConnection = new HubConnectionBuilder()
			.WithUrl(nav.ToAbsoluteUri("/lobby"))
			.WithAutomaticReconnect()
			.Build();


		HubConnection.On<string>("JoinedAsync", id => Console.WriteLine("user joined: " + id));
	}

	public async Task StartAsync() {
		if (HubConnection.State == HubConnectionState.Disconnected)
			await HubConnection.StartAsync().ConfigureAwait(true);
	}
}
