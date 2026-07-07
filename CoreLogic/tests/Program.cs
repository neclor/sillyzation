using session;

namespace CoreLogic.Tests;

internal class Program {
	private static void Main() {
		Console.WriteLine("\n\n\nStarting tests\n");

		lanchTerminalTestApp();

		Console.WriteLine("\nEnding tests");
	}

	private static void lanchTerminalTestApp() {
		LocalSession<Coord> session = new(
			[
				(new HumanPlayer(0, Country.France), [(1,1), (2,1), (1,2)]),
				(new HumanPlayer(1, Country.England), [(4,4), (3,4), (4,3)]),
			],
			[
				((1, 1), new Cell<Coord>((1, 1), "Arlon", Terrain.Tundra, 2000000, [])),
				((2, 1), new Cell<Coord>((2, 1), "Bastogne", Terrain.Tundra, 2500000, [])),
				((3, 1), new Cell<Coord>((3, 1), "Spa", Terrain.Forest, 4000000, [])),
				((4, 1), new Cell<Coord>((4, 1), "Verviers", Terrain.Forest, 4500000, [])),
				((1, 2), new Cell<Coord>((1, 2), "Leuven", Terrain.Plain, 6000000, [])),
				((2, 2), new Cell<Coord>((2, 2), "Namur", Terrain.Plain, 5500000, [])),
				((3, 2), new Cell<Coord>((3, 2), "Liege", Terrain.Forest, 7000000, [])),
				((4, 2), new Cell<Coord>((4, 2), "Bruxelles", Terrain.Swamp, 8000000, [])),
				((1, 3), new Cell<Coord>((1, 3), "Mons", Terrain.Desert, 1500000, [])),
				((2, 3), new Cell<Coord>((2, 3), "Charleroi", Terrain.Savanna, 3000000, [])),
				((3, 3), new Cell<Coord>((3, 3), "Dinant", Terrain.Savanna, 3500000, [])),
				((4, 3), new Cell<Coord>((4, 3), "Ghent", Terrain.Swamp, 5000000, [])),
				((1, 4), new Cell<Coord>((1, 4), "Ostend", Terrain.Desert, 1000000, [])),
				((2, 4), new Cell<Coord>((2, 4), "Brugge", Terrain.Savanna, 4000000, [])),
				((3, 4), new Cell<Coord>((3, 4), "Antwerp", Terrain.Jungle, 7500000, [])),
				((4, 4), new Cell<Coord>((4, 4), "Kortrijk", Terrain.Jungle, 6000000, []))
			],
			[
				// Horizontal Grid Connections
				((1, 1), (2, 1)), ((2, 1), (3, 1)), ((3, 1), (4, 1)),
				((1, 2), (2, 2)), ((2, 2), (3, 2)), ((3, 2), (4, 2)),
				((1, 3), (2, 3)), ((2, 3), (3, 3)), ((3, 3), (4, 3)),
				((1, 4), (2, 4)), ((2, 4), (3, 4)), ((3, 4), (4, 4)),

				// Vertical Grid Connections
				((1, 1), (1, 2)), ((1, 2), (1, 3)), ((1, 3), (1, 4)),
				((2, 1), (2, 2)), ((2, 2), (2, 3)), ((2, 3), (2, 4)),
				((3, 1), (3, 2)), ((3, 2), (3, 3)), ((3, 3), (3, 4)),
				((4, 1), (4, 2)), ((4, 2), (4, 3)), ((4, 3), (4, 4))
			]
		);

		TerminalVersion app = new(session, (4, 4));
		app.start();
	}
}
