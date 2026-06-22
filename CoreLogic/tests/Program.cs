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

	private static void printCell(Terrain terrain, uint? playerId, uint line) {
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

		var player_fg = playerId switch {
			0 => "\u001b[38;5;196m", // Red
			1 => "\u001b[38;5;45m",  // Cyan
			2 => "\u001b[38;5;201m", // Magenta
			3 => "\u001b[38;5;214m", // Orange
			4 => "\u001b[38;5;118m", // Bright Green
			5 => "\u001b[38;5;226m", // Bright Yellow
			6 => "\u001b[38;5;135m", // Purple
			null => "\u001b[38;5;250m", // Light Gray for Neutral
			_ => throw new NotImplementedException(),
		};

		string player_icon = playerId.HasValue ? $"P{playerId}" : " ∙";

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

	private static void printMap(uint playerId, ICore core) {
		for (uint y = 1; y <= 2; y++) {
			for (uint line = 0; line <= 3; line++) {
				for (uint x = 1; x <= 2; x++) {
					var cell = core.getCell(playerId, (x, y));
					if (cell.IsError) {
						return;
					}
					printCell(cell.Value.terrain, cell.Value.owner, line);
				}
				Console.WriteLine();
			}
		}
	}

	private static void test() {
		Core core = new(
			[
				("England", Color.Red),
				("France", Color.Blue),
				("Germany", Color.Gray),
				("Russia", Color.Green),
				("Italy", Color.LightGreen),
				("Spain", Color.Yellow),
				("Belgium", Color.Gold),
				("Netherland", Color.Orange),
				("Austria-Hungary", Color.Orange),
				("Ottoman", Color.Orange),
			],
			[
				((1, 1), new Cell((1, 1), "Bruxelles", Terrain.Swamp, 10000000, [])),
				((2, 1), new Cell((2, 1), "Namur", Terrain.Forest, 3000000, [])),
				((2, 2), new Cell((2, 2), "Liege", Terrain.Desert, 5000000, [])),
				((1, 2), new Cell((1, 2), "Leuven", Terrain.Plain, 2000000, [])),
			],
			[
				((1, 1), (2, 1)),
				((2, 1), (2, 2)),
				((2, 2), (1, 2)),
				((1, 2), (1, 1)),
			]
		);


		printMap(0, core);
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