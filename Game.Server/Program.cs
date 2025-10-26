

namespace Game.Server;

internal static class Program {

	private static void Main(string[] args) {
		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

		builder.Services.AddRazorComponents()
			.AddInteractiveWebAssemblyComponents();

		builder.Services.AddSignalR();

		WebApplication app = builder.Build();

		app.UseDefaultFiles();
		app.UseStaticFiles();
		app.MapStaticAssets();

		app.UseAntiforgery();

		app.MapHub<LobbyHub>("/lobby");

		app.Run();
	}
}
