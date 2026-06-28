using session;

namespace CoreLogic.Tests;


internal class Program {
	private static void Main() {
		Console.WriteLine("\n\n\nStarting tests\n");

		lanchTerminalTestApp();

		Console.WriteLine("\nEnding tests");
	}

	private static void lanchTerminalTestApp() {
		LocalSession<(uint, uint)> session = new(
			[
				(new HumanPlayer(), Country.France, [(1,1)]),
				(new HumanPlayer(), Country.England, [(2,4)]),
			],
			[
				((1, 1), new Cell<(uint, uint)>((1, 1), "Bruxelles", Terrain.Swamp, 10000000, [])),
				((2, 1), new Cell<(uint, uint)>((2, 1), "Namur", Terrain.Forest, 3000000, [])),
				((2, 2), new Cell<(uint, uint)>((2, 2), "Liege", Terrain.Desert, 5000000, [])),
				((1, 2), new Cell<(uint, uint)>((1, 2), "Leuven", Terrain.Plain, 2000000, [])),
				((1, 3), new Cell<(uint, uint)>((1, 2), "Charleroi", Terrain.Savanna, 2000000, [])),
				((2, 3), new Cell<(uint, uint)>((1, 2), "Brugge", Terrain.Jungle, 2000000, [])),
				((1, 4), new Cell<(uint, uint)>((1, 2), "Charleroi", Terrain.Tundra, 2000000, [])),
				((2, 4), new Cell<(uint, uint)>((1, 2), "Brugge", Terrain.Jungle, 2000000, [])),
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

		TerminalVersion app = new(session, (2, 4));
		app.start();
	}
}
