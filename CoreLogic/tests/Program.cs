using CoreLogic.Map;

namespace CoreLogic.Tests;

internal class Program {
	private static void Main() {
		Console.WriteLine("\n\n\nStarting tests\n");

		test();

		Console.WriteLine("\nEnding tests");
	}

	private static void test() {
		Core core = new(
			[
				new PlayerInit("England", Color.Red),
				new PlayerInit("France", Color.Blue),
				new PlayerInit("Germany", Color.Gray),
				new PlayerInit("Russia", Color.Green),
				new PlayerInit("Italy", Color.LightGreen),
				new PlayerInit("Spain", Color.Yellow),
				new PlayerInit("Belgium", Color.Gold),
				new PlayerInit("Netherland", Color.Orange),
				new PlayerInit("Austria-Hungary", Color.Orange),
				new PlayerInit("Ottoman", Color.Orange),
			],
			[
				((1, 1), new Cell((1, 1), "Bruxelles", Terrain.Swamp, 10000000, [])),
				((2, 1), new Cell((2, 1), "Namur", Terrain.Forest, 3000000, [])),
				((2, 2), new Cell((2, 2), "Liege", Terrain.Forest, 5000000, [])),
				((1, 2), new Cell((1, 2), "Leuven", Terrain.Plain, 2000000, [])),
			],
			[
				((1, 1), (2, 1)),
				((2, 1), (2, 2)),
				((2, 2), (1, 2)),
				((1, 2), (1, 1)),
			]
		);


		foreach (uint y in Enumerable.Range(1, 2)) {
			foreach (uint x in Enumerable.Range(1, 2)) {
				var cell = core.getCell(0, (x, y));
				if (cell.IsError) {
					return;
				}
				Console.Write($"{cell.Value.id}, {cell.Value.id})");
			}
			Console.WriteLine();
		}

		// var path = map.getShortestPath((1, 1), (2, 2));
		// if (path.IsError) {
		// 	return;
		// }
		// foreach (var step in path.Value) {
		// 	Console.WriteLine(step.id + " " + step.name);
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