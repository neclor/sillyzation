using CoreLogic.Map;

namespace CoreLogic.Tests;

internal class Program {
	private static void Main() {
		Console.WriteLine("\n\n\nStarting tests\n");

		test();

		Console.WriteLine("\nEnding tests");
	}

	private static void test() {
		Map<(int, int)> map = new(
			[
				((1, 1), new Cell<(int, int)>((1, 1), "Bruxelles", Terrain.Plain)),
				((2, 1), new Cell<(int, int)>((2, 1), "Namur", Terrain.Plain)),
				((2, 2), new Cell<(int, int)>((2, 2), "Liege", Terrain.Plain)),
				((1, 2), new Cell<(int, int)>((1, 2), "Leuven", Terrain.Plain)),
			],
			[
				((1, 1), (2, 1)),
				((2, 1), (2, 2)),
				((2, 2), (1, 2)),
				((1, 2), (1, 1)),
			]
		);

		foreach (int y in Enumerable.Range(0, 2)) {
			foreach (int x in Enumerable.Range(0, 2)) {
				Console.Write($"({x} {y})");
			}
			Console.WriteLine();
		}
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