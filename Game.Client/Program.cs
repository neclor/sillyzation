using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Game.Client.Singletons;

namespace Game.Client;

internal static class Program {

	private static async Task Main(string[] args) {
		WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
		builder.RootComponents.Add<App>("#app");
		builder.RootComponents.Add<HeadOutlet>("head::after");

		builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

		builder.Services.AddSingleton<ConnectionService>();

		await builder.Build().RunAsync().ConfigureAwait(true);
	}
}
