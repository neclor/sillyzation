using ErrorOr;

namespace CoreLogic.Tests;

internal class Program {
	private static void Main() {
		Console.WriteLine("\n\n\nStarting tests\n");

		test();
		// _ = test_error_handling();

		Console.WriteLine("\nEnding tests");
	}

	public static string test_error_handling() {
		ErrorOr<string> d = Error.NotFound();
		if (d.IsError) {
			foreach (Error error in d.Errors) {
				Console.WriteLine(error);
			}
			return "1";
		}
		string test = d.Match(
			x => x,
			errors => {
				foreach (Error error in d.Errors) {
					Console.WriteLine(error);
				}
				return "1";
			}
		);
		Console.WriteLine(test);

		return "";
	}

	private static void test() {
		// Map<uint> map = new(
		// 	[
		// 		(1, new GameCell()),
		// 		(2, new GameCell()),
		// 		(3, new GameCell()),
		// 		(4, new GameCell()),
		// 	],
		// 	[
		// 		(1, 2),
		// 		(2, 3),
		// 		(3, 4),
		// 		(4, 1),
		// 	]
		// );

		// foreach ((uint key, GameCell _) in map.getNeightbours(2)) {
		// 	Console.WriteLine($"{key}");
		// }
	}
}
