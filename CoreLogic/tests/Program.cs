namespace CoreLogic.Tests;

internal class Program {
	private static void Main() {
		Console.WriteLine("\n\n\nStarting tests");

		test();

		Console.WriteLine("Ending tests");
	}

	private static void test() {
		Map<uint> map = new(
			[
				(1, new GameCell()),
				(2, new GameCell()),
				(3, new GameCell()),
				(4, new GameCell()),
			],
			[
				(1, 2),
				(2, 3),
				(3, 4),
				(4, 1),
			]
		);

		foreach ((uint key, GameCell _) in map.getNeightbours(2)) {
			Console.WriteLine($"{key}");
		}
	}
}
