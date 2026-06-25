using System.Text;
using CoreLogic;

internal class TerminalVersion {
	private ICore core { get; }
	private (int x, int y) map_size { get; }
	private Dictionary<uint, IPlayer> players;

	private static readonly (int x, int y) cell_size = (5, 3);

	public TerminalVersion(ICore core, (int, int) map_size) {
		this.core = core;
		this.map_size = map_size;
		players = core.getAllPlayers();
	}

	private string[] printMap() {
		return ["xxxxxxxxxx", "xxxxxxxxxx", "xxxxxxxxxx", "xxxxxxxxxx", "xxxxxxxxxx", "xxxxxxxxxx"];
	}

	private void print(
		string[] contentMenu
	) {
		int longest = (contentMenu.Length != 0)
			? contentMenu.Max((cur) => cur.Length)
			: 0;

		int mapWidth = map_size.x * cell_size.x;
		int mapHeight = map_size.y * cell_size.y;
		int nbLines = contentMenu.Length > mapHeight ? contentMenu.Length : mapHeight;

		string[] map = printMap();

		StringBuilder sb = new();
		_ = sb.Append('╔').Append('═', longest).Append('╦').Append('═', mapWidth).AppendLine("╗");
		foreach ((string line, int i) in contentMenu.Select((value, index) => (value, index))) {
			_ = sb
				.Append('║')
				.Append(line.PadRight(longest))
				.Append('║');
			if (i >= mapHeight) {
				_ = sb
					.Append(' ', mapWidth)
					.AppendLine("║");
			}
			else {
				_ = sb
					.Append(map[i])
					.AppendLine("║");
			}
		}
		if (contentMenu.Length < mapHeight) {
			for (int i = contentMenu.Length; i < mapHeight; i++) {
				_ = sb
					.Append('║')
					.Append(' ', longest)
					.Append('║')
					.Append(map[i])
					.AppendLine("║");
			}
		}
		_ = sb
			.Append('╚')
			.Append('═', longest)
			.Append('╩')
			.Append('═', mapWidth)
			.AppendLine("╝");

		string res = sb.ToString();

		Console.Clear();
		Console.WriteLine(res);
	}

	public void start() {
		print(["Test", "Hello World"]);
		// print(["Test", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World"]);
		// while (true) {
		// 	// foreach ((_, IPlayer player) in players) {
		// 	// 	// printTurn(player, players, core);
		// 	// 	char input = Console.ReadKey().KeyChar;
		// 	// }

		// }
	}
}