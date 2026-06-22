using ErrorOr;

namespace CoreLogic.Tests;


internal class Program {
	private const string RESET = "\u001b[0m";

	private static void Main() {
		Console.WriteLine("\n\n\nStarting tests\n");

		test();

		Console.WriteLine("\nEnding tests");
	}

	private static void printCell(Terrain terrain, uint? playerId, uint line) {
		var cell_color = terrain switch {
			Terrain.Plain => "\u001b[102m",
			Terrain.Desert => "\u001b[103m",
			Terrain.Forest => "\u001b[107m",
			Terrain.Jungle => "\u001b[100m",
			Terrain.Savanna => "\u001b[101m",
			Terrain.Swamp => "\u001b[105m",
			Terrain.Tundra => "\u001b[104m",
			_ => throw new NotImplementedException()
		};
		var player_color = playerId switch {
			0 => "\u001b[40m",
			1 => "\u001b[41m",
			2 => "\u001b[42m",
			3 => "\u001b[43m",
			4 => "\u001b[44m",
			5 => "\u001b[45m",
			6 => "\u001b[46m",
			null => "\u001b[47m",
			_ => throw new NotImplementedException(),
		};

		if (line == 0 || line == 3) {
			Console.Write($"{player_color}      {RESET}");
		}
		else {
			Console.Write($"{player_color} {cell_color}    {player_color} {RESET}");
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