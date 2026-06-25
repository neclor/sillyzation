using ErrorOr;

namespace CoreLogic.Tests;


internal class Program {
	private const string RESET = "\u001b[0m";

	private static void Main() {
		Console.WriteLine("\n\n\nStarting tests\n");

		test();

		Console.WriteLine("\nEnding tests");
	}

	private const string BORDER_COLOR = "\u001b[38;5;244m";

	private static void printCell(Terrain terrain, IPlayer? player, uint line) {
		var cell_color = terrain switch {
			Terrain.Plain => "\u001b[48;5;40m",
			Terrain.Desert => "\u001b[48;5;220m",
			Terrain.Forest => "\u001b[48;5;28m",
			Terrain.Jungle => "\u001b[48;5;34m",
			Terrain.Savanna => "\u001b[48;5;172m",
			Terrain.Swamp => "\u001b[48;5;95m",
			Terrain.Tundra => "\u001b[48;5;74m",
			_ => throw new NotImplementedException()
		};

		var terrain_label = terrain switch {
			Terrain.Plain => "PLN",
			Terrain.Desert => "DSR",
			Terrain.Forest => "FOR",
			Terrain.Jungle => "JGL",
			Terrain.Savanna => "SVN",
			Terrain.Swamp => "SWP",
			Terrain.Tundra => "TUN",
			_ => throw new NotImplementedException()
		};

		var player_fg = player?.color switch {
			Color.Red => "\u001b[38;5;196m",
			Color.LightBlue => "\u001b[38;5;45m",
			Color.Purple => "\u001b[38;5;201m",
			Color.Orange => "\u001b[38;5;214m",
			Color.LightGreen => "\u001b[38;5;118m",
			Color.Yellow => "\u001b[38;5;226m",
			Color.Gray => "\u001b[38;5;250m",
			Color.Gold => "\u001b[38;5;220m",
			Color.Green => "\u001b[38;5;34m",
			Color.Blue => "\u001b[38;5;21m",
			Color.White => "\u001b[38;5;231m",
			Color.Brown => "\u001b[38;5;130m",
			null => "\u001b[38;5;250m",
			_ => throw new NotImplementedException(),
		};

		string player_icon = player != null ? $"P{player.id}" : " ∙";

		switch (line) {
			case 0:
				Console.Write($"{BORDER_COLOR}┌─────┐{RESET}");
				break;
			case 1:
				Console.Write($"{BORDER_COLOR}│{cell_color}\u001b[30m {terrain_label} {RESET}{BORDER_COLOR}│{RESET}");
				break;
			case 2:
				Console.Write($"{BORDER_COLOR}│{cell_color} {player_fg}{player_icon}\u001b[30m  {RESET}{BORDER_COLOR}│{RESET}");
				break;
			case 3:
				Console.Write($"{BORDER_COLOR}└─────┘{RESET}");
				break;
			default:
				break;
		}
	}

	private static void printMap(IPlayer player, Dictionary<uint, IPlayer> players, ICore core) {
		for (uint y = 1; y <= 2; y++) {
			for (uint line = 0; line <= 3; line++) {
				for (uint x = 1; x <= 2; x++) {
					ErrorOr<ICell> cell = core.getCell(player.id, (x, y));
					if (cell.IsError) {
						return;
					}
					uint? owner = cell.Value.owner;
					IPlayer? p = owner != null ? players[owner.Value] : null;
					printCell(cell.Value.terrain, p, line);
				}
				Console.WriteLine();
			}
		}
	}

	private static void printTurn(IPlayer player, Dictionary<uint, IPlayer> players, ICore core) {
		Console.Clear();
		Console.WriteLine("The turn of " + player.name);
		printMap(player, players, core);
	}

	private static void test() {
		Core core = new(
			[
				("England", Color.Red, [(1,1), (1,2)]),
				("France", Color.Blue, [(2,2)]),
				// ("Germany", Color.Gray, []),
				// ("Russia", Color.Green, []),
				// ("Italy", Color.LightGreen, []),
				// ("Spain", Color.Yellow, []),
				// ("Belgium", Color.Gold, []),
				// ("Netherland", Color.Orange, []),
				// ("Austria-Hungary", Color.Orange, []),
				// ("Ottoman", Color.Orange, []),
			],
			[
				((1, 1), new Cell((1, 1), "Bruxelles", Terrain.Swamp, 10000000, [])),
				((2, 1), new Cell((2, 1), "Namur", Terrain.Forest, 3000000, [])),
				((2, 2), new Cell((2, 2), "Liege", Terrain.Desert, 5000000, [])),
				((1, 2), new Cell((1, 2), "Leuven", Terrain.Plain, 2000000, [])),
				((1, 3), new Cell((1, 2), "Charleroi", Terrain.Savanna, 2000000, [])),
				((2, 3), new Cell((1, 2), "Brugge", Terrain.Jungle, 2000000, [])),
				((1, 4), new Cell((1, 2), "Charleroi", Terrain.Tundra, 2000000, [])),
				((2, 4), new Cell((1, 2), "Brugge", Terrain.Jungle, 2000000, [])),
			],
			[
				((1, 1), (2, 1)),
				((2, 1), (2, 2)),
				((2, 2), (1, 2)),
				((1, 2), (1, 1)),
				((1, 2), (1, 3)),
				((1, 3), (1, 4)),
				((2, 2), (2, 3)),
				((2, 3), (2, 4)),
				((1, 4), (2, 4)),
				((1, 3), (2, 3)),
			]
		);


		var app = new TerminalVersion(core, (2, 4));
		app.start();

		// while (true) {
		// 	Dictionary<uint, IPlayer> players = core.getAllPlayers();
		// 	foreach ((_, IPlayer player) in players) {
		// 		printTurn(player, players, core);
		// 		char input = Console.ReadKey().KeyChar;
		// 		Console.WriteLine("key => " + (int) input);
		// 		switch ((int) input) {
		// 			case 27:
		// 				return;
		// 			default:
		// 				break;
		// 		}
		// 		_ = Console.ReadKey();
		// 	}
		// }
	}
}

// public static string test_error_handling() {
// 	ErrorOr<Tuple<int, int>> d = Error.NotFound();
// 	if (d.IsError) {
// 		foreach (Error error in d.Errors) {
// 			Console.WriteLine(error);
// 		}
// 		return "1";
// 	}
// 	Tuple<int, int> test = d.Match(
// 		x => x,
// 		errors => {
// 			foreach (Error error in d.Errors) {
// 				Console.WriteLine(error);
// 			}
// 			return "1";
// 		}
// 	);
// 	Console.WriteLine(test);

// 	return "";
// }