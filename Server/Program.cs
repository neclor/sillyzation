using Server.Hubs;
using Server.Singletons;

namespace Server;

internal static class Program {

	private static void Main(string[] args) {
		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
		builder.Services.AddRazorComponents()
			.AddInteractiveWebAssemblyComponents();
		builder.Services.AddSignalR();

		builder.Services.AddSingleton<GameManager>();

		WebApplication app = builder.Build();
		app.UseDefaultFiles();
		app.UseStaticFiles();
		app.MapStaticAssets();
		app.UseAntiforgery();

		app.MapHub<GameHub>("/lobby");

		app.MapFallbackToFile("index.html");

		app.Run();
	}
}
