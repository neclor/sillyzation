using System.Text;
using session;
using CoreLogic;
using ErrorOr;
using System.Globalization;

internal class TerminalVersion {
	private ISession<(uint, uint)> session { get; }
	private (int x, int y) map_size { get; }
	private Dictionary<uint, IPlayer> players;

	private static readonly (int x, int y) cell_size = (5, 4);
	private const int minMenuWidth = 32;

	private const string RESET = "\x1b[0m";
	private static readonly Dictionary<Terrain, string[][]> backgrounds = new() {
		{
			Terrain.Plain, [
				["\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[38;5;71;48;5;107m.\x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m"],
				["\x1b[48;5;106m \x1b[0m", "\x1b[38;5;142;48;5;106m~\x1b[0m", "\x1b[48;5;106m \x1b[0m", "\x1b[48;5;106m \x1b[0m", "\x1b[48;5;106m \x1b[0m"],
				["\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[48;5;107m \x1b[0m", "\x1b[38;5;71;48;5;107m.\x1b[0m", "\x1b[48;5;107m \x1b[0m"],
				["\x1b[48;5;100m \x1b[0m", "\x1b[48;5;100m \x1b[0m", "\x1b[48;5;100m \x1b[0m", "\x1b[48;5;100m \x1b[0m", "\x1b[48;5;100m \x1b[0m"]
			]
		},
		{
			Terrain.Forest, [
				["\x1b[38;5;22;48;5;28m▲\x1b[0m", "\x1b[38;5;28;48;5;22m \x1b[0m", "\x1b[38;5;22;48;5;28m▲\x1b[0m", "\x1b[38;5;28;48;5;22m \x1b[0m", "\x1b[38;5;22;48;5;28m▲\x1b[0m"],
				["\x1b[38;5;34;48;5;22m \x1b[0m", "\x1b[38;5;22;48;5;34m▲\x1b[0m", "\x1b[38;5;34;48;5;22m \x1b[0m", "\x1b[38;5;22;48;5;34m▲\x1b[0m", "\x1b[38;5;34;48;5;22m \x1b[0m"],
				["\x1b[38;5;22;48;5;28m▲\x1b[0m", "\x1b[38;5;28;48;5;22m \x1b[0m", "\x1b[38;5;22;48;5;28m▲\x1b[0m", "\x1b[38;5;28;48;5;22m \x1b[0m", "\x1b[38;5;22;48;5;28m▲\x1b[0m"],
				["\x1b[38;5;58;48;5;22m▄\x1b[0m", "\x1b[38;5;58;48;5;22m▄\x1b[0m", "\x1b[38;5;58;48;5;22m▄\x1b[0m", "\x1b[38;5;58;48;5;22m▄\x1b[0m", "\x1b[38;5;58;48;5;22m▄\x1b[0m"]
			]
		},
		{
			Terrain.Desert, [
				["\x1b[48;5;221m \x1b[0m", "\x1b[48;5;221m \x1b[0m", "\x1b[38;5;214;48;5;221m~\x1b[0m", "\x1b[48;5;221m \x1b[0m", "\x1b[48;5;221m \x1b[0m"],
				["\x1b[38;5;221;48;5;222m▄\x1b[0m", "\x1b[38;5;221;48;5;222m█\x1b[0m", "\x1b[48;5;222m \x1b[0m", "\x1b[48;5;222m \x1b[0m", "\x1b[38;5;221;48;5;222m▄\x1b[0m"],
				["\x1b[48;5;214m \x1b[0m", "\x1b[48;5;214m \x1b[0m", "\x1b[38;5;166;48;5;214m~\x1b[0m", "\x1b[48;5;214m \x1b[0m", "\x1b[48;5;214m \x1b[0m"],
				["\x1b[48;5;130m \x1b[0m", "\x1b[48;5;130m \x1b[0m", "\x1b[48;5;130m \x1b[0m", "\x1b[48;5;130m \x1b[0m", "\x1b[48;5;130m \x1b[0m"]
			]
		},
		{
			Terrain.Tundra, [
				["\x1b[48;5;253m \x1b[0m", "\x1b[38;5;255;48;5;253m*\x1b[0m", "\x1b[48;5;253m \x1b[0m", "\x1b[48;5;253m \x1b[0m", "\x1b[48;5;253m \x1b[0m"],
				["\x1b[48;5;195m \x1b[0m", "\x1b[48;5;195m \x1b[0m", "\x1b[38;5;255;48;5;195m-\x1b[0m", "\x1b[48;5;195m \x1b[0m", "\x1b[38;5;255;48;5;195m*\x1b[0m"],
				["\x1b[38;5;250;48;5;254m▄\x1b[0m", "\x1b[48;5;254m \x1b[0m", "\x1b[48;5;254m \x1b[0m", "\x1b[38;5;250;48;5;254m▄\x1b[0m", "\x1b[48;5;254m \x1b[0m"],
				["\x1b[48;5;245m \x1b[0m", "\x1b[48;5;245m \x1b[0m", "\x1b[48;5;245m \x1b[0m", "\x1b[48;5;245m \x1b[0m", "\x1b[48;5;245m \x1b[0m"]
			]
		},
		{
			Terrain.Savanna, [
				["\x1b[48;5;142m \x1b[0m", "\x1b[38;5;100;48;5;142m┵\x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m"],
				["\x1b[48;5;136m \x1b[0m", "\x1b[48;5;136m \x1b[0m", "\x1b[48;5;136m \x1b[0m", "\x1b[38;5;94;48;5;136m┵\x1b[0m", "\x1b[48;5;136m \x1b[0m"],
				["\x1b[38;5;100;48;5;142m┵\x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[48;5;142m \x1b[0m", "\x1b[38;5;100;48;5;142m┵\x1b[0m"],
				["\x1b[48;5;94m \x1b[0m", "\x1b[48;5;94m \x1b[0m", "\x1b[48;5;94m \x1b[0m", "\x1b[48;5;94m \x1b[0m", "\x1b[48;5;94m \x1b[0m"]
			]
		},
		{
			Terrain.Swamp, [
				["\x1b[48;5;59m \x1b[0m", "\x1b[38;5;236;48;5;59m░\x1b[0m", "\x1b[48;5;59m \x1b[0m", "\x1b[48;5;59m \x1b[0m", "\x1b[38;5;236;48;5;59m░\x1b[0m"],
				["\x1b[38;5;22;48;5;30m█\x1b[0m", "\x1b[48;5;30m \x1b[0m", "\x1b[38;5;23;48;5;30m~\x1b[0m", "\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m \x1b[0m"],
				["\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m \x1b[0m", "\x1b[48;5;30m \x1b[0m", "\x1b[38;5;22;48;5;30m█\x1b[0m", "\x1b[48;5;30m \x1b[0m"],
				["\x1b[48;5;23m \x1b[0m", "\x1b[48;5;23m \x1b[0m", "\x1b[48;5;23m \x1b[0m", "\x1b[48;5;23m \x1b[0m", "\x1b[48;5;23m \x1b[0m"]
			]
		},
		{
			Terrain.Jungle, [
				["\x1b[38;5;28;48;5;34m♣\x1b[0m", "\x1b[48;5;34m \x1b[0m", "\x1b[38;5;22;48;5;34m▓\x1b[0m", "\x1b[48;5;34m \x1b[0m", "\x1b[38;5;28;48;5;34m♣\x1b[0m"],
				["\x1b[48;5;28m \x1b[0m", "\x1b[38;5;34;48;5;28m♣\x1b[0m", "\x1b[48;5;28m \x1b[0m", "\x1b[48;5;28m \x1b[0m", "\x1b[48;5;28m \x1b[0m"],
				["\x1b[38;5;22;48;5;34m▓\x1b[0m", "\x1b[48;5;34m \x1b[0m", "\x1b[48;5;34m \x1b[0m", "\x1b[38;5;28;48;5;34m♣\x1b[0m", "\x1b[48;5;34m \x1b[0m"],
				["\x1b[48;5;22m \x1b[0m", "\x1b[48;5;22m \x1b[0m", "\x1b[48;5;22m \x1b[0m", "\x1b[48;5;22m \x1b[0m", "\x1b[48;5;22m \x1b[0m"]
			]
		},
	};

	public TerminalVersion(ISession<(uint, uint)> session, (int, int) map_size) {
		this.session = session;
		this.map_size = map_size;
		players = session.getAllPlayers();
	}

	public static string getAnsiBackgroundColor(Color color) => color switch {
		Color.Red => "\u001b[41m",
		Color.Gold => "\u001b[48;5;214m",
		Color.Orange => "\u001b[48;5;202m",
		Color.Yellow => "\u001b[43m",
		Color.LightGreen => "\u001b[48;5;120m",
		Color.DarkGreen => "\u001b[48;5;22m",
		Color.Green => "\u001b[42m",
		Color.LightBlue => "\u001b[46m",
		Color.Blue => "\u001b[44m",
		Color.Purple => "\u001b[45m",
		Color.White => "\u001b[47m",
		Color.Gray => "\u001b[48;5;244m",
		Color.Brown => "\u001b[48;5;94m",
		_ => "\u001b[0m"
	};

	private List<string> printMap(IPlayer player) {
		var cells = new (Terrain terrain, uint? ownership)[map_size.x][];
		for (uint x = 0; x < map_size.x; x++) {
			cells[x] = new (Terrain terrain, uint? ownership)[map_size.y];
			for (uint y = 0; y < map_size.y; y++) {
				ErrorOr<ICell<(uint, uint)>> cell = session.getCell(player.id, (x + 1, y + 1));
				if (!cell.IsError) {
					cells[x][y] = (cell.Value.terrain, cell.Value.owner);
				}
				else {
					cells[x][y] = (Terrain.Plain, null);
				}
			}
		}

		const string reset = "\u001b[0m";

		List<List<string[]>> map = [];
		for (uint y = 0; y < map_size.y; y++) {
			for (uint yc = 0; yc < cell_size.y; yc++) {
				List<string[]> line = [];
				for (uint x = 0; x < map_size.x; x++) {
					string[] cell_line = [.. backgrounds[cells[x][y].terrain][yc]];
					uint? current_owner = cells[x][y].ownership;

					if (current_owner.HasValue) {
						string p_color = getAnsiBackgroundColor(players[current_owner.Value].color);
						if (yc == 0) {
							if (!(y > 0) || cells[x][y - 1].ownership != current_owner) {
								for (int i = 0; i < cell_line.Length; i++) {
									cell_line[i] = $"{p_color} {reset}";
								}
							}
						}
						if (yc == cell_size.y - 1) {
							if (!(y < map_size.y - 1) || cells[x][y + 1].ownership != current_owner) {
								for (int i = 0; i < cell_line.Length; i++) {
									cell_line[i] = $"{p_color} {reset}";
								}
							}
						}
						if (!(x > 0) || cells[x - 1][y].ownership != current_owner) {
							cell_line[0] = $"{p_color} {reset}";
						}
						if (!(x < map_size.x - 1) || cells[x + 1][y].ownership != current_owner) {
							cell_line[^1] = $"{p_color} {reset}";
						}
					}

					line.AddRange(cell_line);
				}
				map.Add(line);
			}
		}

		return [.. map.Select(
			(line) => line.Aggregate("", (acc, cur) => acc + string.Concat(cur)))
		];
	}

	private void print((string content, string color)[] contentMenu, IPlayer player) {
		int longest = (contentMenu.Length != 0)
			? contentMenu.Max((cur) => cur.content.Length)
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
		foreach (((string content, string color), int i) in contentMenu.Select((value, index) => (value, index))) {
			_ = sb.Append(CultureInfo.InvariantCulture, $"║{color}{content.PadRight(menuWidth)}{RESET}║");
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
					.AppendLine(CultureInfo.InvariantCulture, $"║{map[i]}║");
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
		print([("Test", ""), ("Hello World", "\x1b[48;5;214m")], testPlayer);
		// print(["Test", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World", "Hello World"], testPlayer);
		// while (true) {
		// 	// foreach ((_, IPlayer player) in players) {
		// 	// 	// printTurn(player, players, core);
		// 	// 	char input = Console.ReadKey().KeyChar;
		// 	// }

		// }
	}
}