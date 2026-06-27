using System.Text;
using CoreLogic;
using ErrorOr;

internal class TerminalVersion {
	private ICore core { get; }
	private (int x, int y) map_size { get; }
	private Dictionary<uint, IPlayer> players;

	private static readonly (int x, int y) cell_size = (5, 3);
	private const int minMenuWidth = 32;

	private static readonly Dictionary<Terrain, string[][]> backgrounds = new() {
		{
			Terrain.Plain, [
				["\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m"],
				["\x1b[48;5;106m \x1b[0m", "\x1b[48;5;106m \x1b[0m", "\x1b[48;5;106m \x1b[0m", "\x1b[48;5;106m \x1b[0m", "\x1b[48;5;106m \x1b[0m"],
				["\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m"]
			]
		},
		{
			Terrain.Forest, [
				["\x1b[38;5;22;48;5;28m \x1b[0m", "\x1b[38;5;22;48;5;28m▲\x1b[0m", "\x1b[38;5;22;48;5;28m \x1b[0m", "\x1b[38;5;22;48;5;28m▲\x1b[0m", "\x1b[38;5;22;48;5;28m \x1b[0m"],
				["\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m▲\x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m"],
				["\x1b[38;5;22;48;5;28m \x1b[0m", "\x1b[38;5;22;48;5;28m▲\x1b[0m", "\x1b[38;5;22;48;5;28m \x1b[0m", "\x1b[38;5;22;48;5;28m \x1b[0m", "\x1b[38;5;22;48;5;28m \x1b[0m"]
			]
		},
		{
			Terrain.Desert, [
				["\x1b[48;5;221m \x1b[0m", "\x1b[48;5;221m \x1b[0m", "\x1b[48;5;221m \x1b[0m", "\x1b[48;5;221m \x1b[0m", "\x1b[48;5;221m \x1b[0m"],
				["\x1b[48;5;222m \x1b[0m", "\x1b[48;5;222m \x1b[0m", "\x1b[48;5;222m \x1b[0m", "\x1b[48;5;222m \x1b[0m", "\x1b[48;5;222m \x1b[0m"],
				["\x1b[48;5;214m \x1b[0m", "\x1b[48;5;214m \x1b[0m", "\x1b[48;5;214m \x1b[0m", "\x1b[48;5;214m \x1b[0m", "\x1b[48;5;214m \x1b[0m"]
			]
		},
		{
			Terrain.Tundra, [
				["\x1b[48;5;253m \x1b[0m", "\x1b[48;5;253m \x1b[0m", "\x1b[48;5;253m \x1b[0m", "\x1b[48;5;253m \x1b[0m", "\x1b[48;5;253m \x1b[0m"],
				["\x1b[48;5;195m \x1b[0m", "\x1b[48;5;195m \x1b[0m", "\x1b[48;5;195m \x1b[0m", "\x1b[48;5;195m \x1b[0m", "\x1b[48;5;195m \x1b[0m"],
				["\x1b[48;5;254m \x1b[0m", "\x1b[48;5;254m \x1b[0m", "\x1b[48;5;254m \x1b[0m", "\x1b[48;5;254m \x1b[0m", "\x1b[48;5;254m \x1b[0m"]
			]
		},
		{
			Terrain.Savanna, [
				["\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m"],
				["\x1b[48;5;136m \x1b[0m", "\x1b[48;5;136m \x1b[0m", "\x1b[48;5;136m \x1b[0m", "\x1b[48;5;136m \x1b[0m", "\x1b[48;5;136m \x1b[0m"],
				["\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m"]
			]
		},
		{
			Terrain.Swamp, [
				["\x1b[48;5;59m \x1b[0m", "\x1b[48;5;59m \x1b[0m", "\x1b[48;5;59m \x1b[0m", "\x1b[48;5;59m \x1b[0m", "\x1b[48;5;59m \x1b[0m"],
				["\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m█\x1b[0m", "\x1b[48;5;30m \x1b[0m"],
				["\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m█\x1b[0m", "\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m \x1b[0m"]
			]
		},
		{
			Terrain.Jungle, [
				["\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m♣\x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m"],
				["\x1b[48;5;28m \x1b[0m", "\x1b[48;5;28m \x1b[0m", "\x1b[48;5;28m \x1b[0m", "\x1b[48;5;28m \x1b[0m", "\x1b[48;5;28m \x1b[0m"],
				["\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m♣\x1b[0m", "\x1b[38;5;22;48;5;34m \x1b[0m"]
			]
		},
	};

	public TerminalVersion(ICore core, (int, int) map_size) {
		this.core = core;
		this.map_size = map_size;
		players = core.getAllPlayers();
	}

	private List<string> printMap(IPlayer player) {
		Terrain[][] terrains = new Terrain[map_size.x][];
		for (uint x = 0; x < map_size.x; x++) {
			terrains[x] = new Terrain[map_size.y];
			for (uint y = 0; y < map_size.y; y++) {
				ErrorOr<ICell> cell = core.getCell(player.id, (x + 1, y + 1));
				Terrain terrain = cell.IsError ? Terrain.Plain : cell.Value.terrain;
				terrains[x][y] = terrain;
			}
		}

		List<List<string[]>> map = [];
		for (uint y = 0; y < map_size.y; y++) {
			for (uint yc = 0; yc < cell_size.y; yc++) {
				List<string[]> line = [];
				for (uint x = 0; x < map_size.x; x++) {
					string[] cell_line = backgrounds[terrains[x][y]][yc];
					line.AddRange(cell_line);
				}
				map.Add(line);
			}
		}

		return [.. map.Select(
			(line) => line.Aggregate("", (acc, cur) => acc + string.Concat(cur)))
		];
	}

	private void print(string[] contentMenu, IPlayer player) {
		int longest = (contentMenu.Length != 0)
			? contentMenu.Max((cur) => cur.Length)
			: 0;
		int menuWidth = Math.Max(longest, minMenuWidth);
		int mapWidth = map_size.x * cell_size.x;
		int mapHeight = map_size.y * cell_size.y;
		int nbLines = contentMenu.Length > mapHeight ? contentMenu.Length : mapHeight;

		List<string> map = printMap(player);

		StringBuilder sb = new();
		_ = sb
			.Append('╔')
			.Append('═', menuWidth)
			.Append('╦')
			.Append('═', mapWidth)
			.AppendLine("╗");
		foreach ((string line, int i) in contentMenu.Select((value, index) => (value, index))) {
			_ = sb
				.Append('║')
				.Append(line.PadRight(menuWidth))
				.Append('║');
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
					.Append('║')
					.Append(map[i])
					.AppendLine("║");
			}
		}
		_ = sb
			.Append('╚')
			.Append('═', menuWidth)
			.Append('╩')
			.Append('═', mapWidth)
			.AppendLine("╝");

		string res = sb.ToString();

		Console.Clear();
		Console.WriteLine(res);
	}

	public void start() {
		IPlayer testPlayer = players[1];
		print(["Test", "Hello World"], testPlayer);
		// print(["Test", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World"], testPlayer);
		// while (true) {
		// 	// foreach ((_, IPlayer player) in players) {
		// 	// 	// printTurn(player, players, core);
		// 	// 	char input = Console.ReadKey().KeyChar;
		// 	// }

		// }
	}
}