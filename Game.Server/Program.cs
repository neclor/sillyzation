using Game.Server.Singletons;
using Game.Server.Hubs;

namespace Game.Server;

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
		app.MapFallbackToPage("/_Host");

		app.Run();
	}
}
