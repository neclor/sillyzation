using System.Text;
using session;
using CoreLogic;
using ErrorOr;
using System.Globalization;
using AC = AnsiColors;

internal class TerminalVersion {
	private ISession<(uint, uint)> session { get; }
	private (int x, int y) map_size { get; }
	private Dictionary<uint, IPlayer> players;

	private static readonly (int x, int y) cell_size = (5, 4);
	private const int minMenuWidth = 32;

	private readonly SimpleMenu menu = new("Choose your option :", "", true, [
		new ExecuteOption("End Turn", () => MenuResult.ExitAll),
		new DynamicMenu<string>(
			"Select Unit",
			"Move Units",
			[
				new GoBackOption("Go Back"),
			],
			(arg) => new DynamicMenu<(uint, uint)>(
				$"From {arg} to:", $"From {arg}", [
					new GoBackOption("Go Back"),
				],
				((uint x, uint y) c) =>
					new ExecuteOption($"Unit {arg} to ({c.x}, {c.y})", () => MenuResult.GoBackToRoot
				),
				() => [(1,1),(1,2),(1,3),(1,4)]
			),
			() => ["Hello", "World", "Stupid"]
		),
		new SimpleMenu("Unit Queue actions:", "Unit Queue", false, [
			new GoBackOption("Go Back"),
			new ExecuteOption("New Unit Queue", () => MenuResult.GoBackToRoot),
			new ExecuteOption("Add new unit to unit Queue", () => MenuResult.GoBackToRoot),
			new ExecuteOption("Deploy Unit Queue", () => MenuResult.GoBackToRoot),
		]),
	]);

	public TerminalVersion(ISession<(uint, uint)> session, (int, int) map_size) {
		this.session = session;
		this.map_size = map_size;
		players = session.getAllPlayers();
	}

	private void print((string content, string color)[] contentMenu, IPlayer player) {
		int longest = (contentMenu.Length != 0)
			? contentMenu.Max((cur) => cur.content.Length)
			: 0;
		int menuWidth = Math.Max(longest, minMenuWidth);
		int mapWidth = map_size.x * cell_size.x * 2;
		int mapHeight = map_size.y * cell_size.y;
		int nbLines = contentMenu.Length > mapHeight ? contentMenu.Length : mapHeight;

		List<string> map = TerminalMap.printMap(
			map_size,
			playerId => players[playerId].color,
			c => session.getCell(player.id, c)
		);

		StringBuilder sb = new();
		_ = sb
			.Append('╔')
			.Append('═', menuWidth)
			.Append('╦')
			.Append('═', mapWidth)
			.AppendLine("╗");
		foreach (((string content, string color), int i) in contentMenu.Select((value, index) => (value, index))) {
			_ = sb.Append(CultureInfo.InvariantCulture, $"║{color}{content.PadRight(menuWidth)}{AC.RESET}║");
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
					.Append(' ', menuWidth)
					.AppendLine(CultureInfo.InvariantCulture, $"║{map[i]}║");
			}
		}
		_ = sb
			.Append('╚')
			.Append('═', menuWidth)
			.Append('╩')
			.Append('═', mapWidth)
			.AppendLine("╝");

		string res = sb.ToString();
		clear();
		Console.WriteLine(res);
	}

	private static void clear() {
		Console.Write(new string('\n', Console.WindowHeight));
		Console.Write("\x1b[H");
	}

	public void start() {
		IPlayer testPlayer = players[1];
		_ = menu.display();
		print([("Test", ""), ("Hello World", "\x1b[48;5;214m")], testPlayer);
		// print(["Test", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World"], testPlayer);
		// while (true) {
		// 	// foreach ((_, IPlayer player) in players) {
		// 	// 	// printTurn(player, players, core);
		// 	// 	char input = Console.ReadKey().KeyChar;
		// 	// }

		// }
	}
}