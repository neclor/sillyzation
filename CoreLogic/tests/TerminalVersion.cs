using System.Text;
using session;
using CoreLogic;
using ErrorOr;
using System.Globalization;
using AC = AnsiiColors;

internal class TerminalVersion {
	private ISession<(uint, uint)> session { get; }
	private (int x, int y) map_size { get; }
	private Dictionary<uint, IPlayer> players;

	private static readonly (int x, int y) cell_size = (5, 4);
	private const int minMenuWidth = 32;

	private static readonly Dictionary<Terrain, string[][]> backgrounds = new() {
		{
			Terrain.Plain, [
				[$"{AC.BG_GRASS_MED} {AC.RESET}", $"{AC.BG_GRASS_MED} {AC.RESET}", $"{AC.FG_OLIVE_GREEN}{AC.BG_GRASS_MED}.{AC.RESET}", $"{AC.BG_GRASS_MED} {AC.RESET}", $"{AC.BG_GRASS_MED} {AC.RESET}"],
				[$"{AC.BG_GRASS_LIGHT} {AC.RESET}", $"{AC.FG_DARK_YELLOW}{AC.BG_GRASS_LIGHT}~{AC.RESET}", $"{AC.BG_GRASS_LIGHT} {AC.RESET}", $"{AC.BG_GRASS_LIGHT} {AC.RESET}", $"{AC.BG_GRASS_LIGHT} {AC.RESET}"],
				[$"{AC.BG_GRASS_MED} {AC.RESET}", $"{AC.BG_GRASS_MED} {AC.RESET}", $"{AC.BG_GRASS_MED} {AC.RESET}", $"{AC.FG_OLIVE_GREEN}{AC.BG_GRASS_MED}.{AC.RESET}", $"{AC.BG_GRASS_MED} {AC.RESET}"],
				[$"{AC.BG_SANDY_BROWN} {AC.RESET}", $"{AC.BG_SANDY_BROWN} {AC.RESET}", $"{AC.BG_SANDY_BROWN} {AC.RESET}", $"{AC.BG_SANDY_BROWN} {AC.RESET}", $"{AC.BG_SANDY_BROWN} {AC.RESET}"]
			]
		},
		{
			Terrain.Forest, [
				[$"{AC.FG_DARK_GREEN}{AC.BG_FOREST_MED}▲{AC.RESET}", $"{AC.FG_MED_GREEN}{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.FG_DARK_GREEN}{AC.BG_FOREST_MED}▲{AC.RESET}", $"{AC.FG_MED_GREEN}{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.FG_DARK_GREEN}{AC.BG_FOREST_MED}▲{AC.RESET}"],
				[$"{AC.FG_LIGHT_GREEN}{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.FG_DARK_GREEN}{AC.BG_FOREST_LIGHT}▲{AC.RESET}", $"{AC.FG_LIGHT_GREEN}{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.FG_DARK_GREEN}{AC.BG_FOREST_LIGHT}▲{AC.RESET}", $"{AC.FG_LIGHT_GREEN}{AC.BG_FOREST_DARK} {AC.RESET}"],
				[$"{AC.FG_DARK_GREEN}{AC.BG_FOREST_MED}▲{AC.RESET}", $"{AC.FG_MED_GREEN}{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.FG_DARK_GREEN}{AC.BG_FOREST_MED}▲{AC.RESET}", $"{AC.FG_MED_GREEN}{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.FG_DARK_GREEN}{AC.BG_FOREST_MED}▲{AC.RESET}"],
				[$"{AC.FG_MUD_BROWN}{AC.BG_FOREST_DARK}▄{AC.RESET}", $"{AC.FG_MUD_BROWN}{AC.BG_FOREST_DARK}▄{AC.RESET}", $"{AC.FG_MUD_BROWN}{AC.BG_FOREST_DARK}▄{AC.RESET}", $"{AC.FG_MUD_BROWN}{AC.BG_FOREST_DARK}▄{AC.RESET}", $"{AC.FG_MUD_BROWN}{AC.BG_FOREST_DARK}▄{AC.RESET}"]
			]
		},
		{
			Terrain.Desert, [
				[$"{AC.BG_DESERT_LIGHT} {AC.RESET}", $"{AC.BG_DESERT_LIGHT} {AC.RESET}", $"{AC.FG_LIGHT_ORANGE}{AC.BG_DESERT_LIGHT}~{AC.RESET}", $"{AC.BG_DESERT_LIGHT} {AC.RESET}", $"{AC.BG_DESERT_LIGHT} {AC.RESET}"],
				[$"{AC.FG_SAND_YELLOW}{AC.BG_DESERT_SAND}▄{AC.RESET}", $"{AC.FG_SAND_YELLOW}{AC.BG_DESERT_SAND}█{AC.RESET}", $"{AC.BG_DESERT_SAND} {AC.RESET}", $"{AC.BG_DESERT_SAND} {AC.RESET}", $"{AC.FG_SAND_YELLOW}{AC.BG_DESERT_SAND}▄{AC.RESET}"],
				[$"{AC.BG_DESERT_MID} {AC.RESET}", $"{AC.BG_DESERT_MID} {AC.RESET}", $"{AC.FG_GOLD_ORANGE}{AC.BG_DESERT_MID}~{AC.RESET}", $"{AC.BG_DESERT_MID} {AC.RESET}", $"{AC.BG_DESERT_MID} {AC.RESET}"],
				[$"{AC.BG_DARK_BROWN} {AC.RESET}", $"{AC.BG_DARK_BROWN} {AC.RESET}", $"{AC.BG_DARK_BROWN} {AC.RESET}", $"{AC.BG_DARK_BROWN} {AC.RESET}", $"{AC.BG_DARK_BROWN} {AC.RESET}"]
			]
		},
		{
			Terrain.Tundra, [
				[$"{AC.BG_TUNDRA_LIGHT} {AC.RESET}", $"{AC.FG_WHITE}{AC.BG_TUNDRA_LIGHT}*{AC.RESET}", $"{AC.BG_TUNDRA_LIGHT} {AC.RESET}", $"{AC.BG_TUNDRA_LIGHT} {AC.RESET}", $"{AC.BG_TUNDRA_LIGHT} {AC.RESET}"],
				[$"{AC.BG_ICE_BLUE} {AC.RESET}", $"{AC.BG_ICE_BLUE} {AC.RESET}", $"{AC.FG_WHITE}{AC.BG_ICE_BLUE}-{AC.RESET}", $"{AC.BG_ICE_BLUE} {AC.RESET}", $"{AC.FG_WHITE}{AC.BG_ICE_BLUE}*{AC.RESET}"],
				[$"{AC.FG_LIGHT_GRAY}{AC.BG_TUNDRA_SNOW}▄{AC.RESET}", $"{AC.BG_TUNDRA_SNOW} {AC.RESET}", $"{AC.BG_TUNDRA_SNOW} {AC.RESET}", $"{AC.FG_LIGHT_GRAY}{AC.BG_TUNDRA_SNOW}▄{AC.RESET}", $"{AC.BG_TUNDRA_SNOW} {AC.RESET}"],
				[$"{AC.BG_TUNDRA_DARK} {AC.RESET}", $"{AC.BG_TUNDRA_DARK} {AC.RESET}", $"{AC.BG_TUNDRA_DARK} {AC.RESET}", $"{AC.BG_TUNDRA_DARK} {AC.RESET}", $"{AC.BG_TUNDRA_DARK} {AC.RESET}"]
			]
		},
		{
			Terrain.Savanna, [
				[$"{AC.BG_SAVANNA_DRY} {AC.RESET}", $"{AC.FG_SANDY_BROWN}{AC.BG_SAVANNA_DRY}┵{AC.RESET}", $"{AC.BG_SAVANNA_DRY} {AC.RESET}", $"{AC.BG_SAVANNA_DRY} {AC.RESET}", $"{AC.BG_SAVANNA_DRY} {AC.RESET}"],
				[$"{AC.BG_SAVANNA_MED} {AC.RESET}", $"{AC.BG_SAVANNA_MED} {AC.RESET}", $"{AC.BG_SAVANNA_MED} {AC.RESET}", $"{AC.FG_DARK_BROWN}{AC.BG_SAVANNA_MED}┵{AC.RESET}", $"{AC.BG_SAVANNA_MED} {AC.RESET}"],
				[$"{AC.FG_SANDY_BROWN}{AC.BG_SAVANNA_DRY}┵{AC.RESET}", $"{AC.BG_SAVANNA_DRY} {AC.RESET}", $"{AC.BG_SAVANNA_DRY} {AC.RESET}", $"{AC.BG_SAVANNA_DRY} {AC.RESET}", $"{AC.FG_SANDY_BROWN}{AC.BG_SAVANNA_DRY}┵{AC.RESET}"],
				[$"{AC.BG_DARK_BROWN} {AC.RESET}", $"{AC.BG_DARK_BROWN} {AC.RESET}", $"{AC.BG_DARK_BROWN} {AC.RESET}", $"{AC.BG_DARK_BROWN} {AC.RESET}", $"{AC.BG_DARK_BROWN} {AC.RESET}"]
			]
		},
		{
			Terrain.Swamp, [
				[$"{AC.BG_MUD_GRAY} {AC.RESET}", $"{AC.FG_CHARCOAL}{AC.BG_MUD_GRAY}░{AC.RESET}", $"{AC.BG_MUD_GRAY} {AC.RESET}", $"{AC.BG_MUD_GRAY} {AC.RESET}", $"{AC.FG_CHARCOAL}{AC.BG_MUD_GRAY}░{AC.RESET}"],
				[$"{AC.FG_DARK_GREEN}{AC.BG_SWAMP_WATER}█{AC.RESET}", $"{AC.BG_SWAMP_WATER} {AC.RESET}", $"{AC.FG_DEEP_BLUE}{AC.BG_SWAMP_WATER}~{AC.RESET}", $"{AC.BG_SWAMP_WATER} {AC.RESET}", $"{AC.BG_SWAMP_WATER} {AC.RESET}"],
				[$"{AC.BG_SWAMP_WATER} {AC.RESET}", $"{AC.BG_SWAMP_WATER} {AC.RESET}", $"{AC.BG_SWAMP_WATER} {AC.RESET}", $"{AC.FG_DARK_GREEN}{AC.BG_SWAMP_WATER}█{AC.RESET}", $"{AC.BG_SWAMP_WATER} {AC.RESET}"],
				[$"{AC.BG_DEEP_BLUE} {AC.RESET}", $"{AC.BG_DEEP_BLUE} {AC.RESET}", $"{AC.BG_DEEP_BLUE} {AC.RESET}", $"{AC.BG_DEEP_BLUE} {AC.RESET}", $"{AC.BG_DEEP_BLUE} {AC.RESET}"]
			]
		},
		{
			Terrain.Jungle, [
				[$"{AC.FG_MED_GREEN}{AC.BG_FOREST_LIGHT}♣{AC.RESET}", $"{AC.BG_FOREST_LIGHT} {AC.RESET}", $"{AC.FG_DARK_GREEN}{AC.BG_FOREST_LIGHT}▓{AC.RESET}", $"{AC.BG_FOREST_LIGHT} {AC.RESET}", $"{AC.FG_MED_GREEN}{AC.BG_FOREST_LIGHT}♣{AC.RESET}"],
				[$"{AC.BG_FOREST_MED} {AC.RESET}", $"{AC.FG_LIGHT_GREEN}{AC.BG_FOREST_MED}♣{AC.RESET}", $"{AC.BG_FOREST_MED} {AC.RESET}", $"{AC.BG_FOREST_MED} {AC.RESET}", $"{AC.BG_FOREST_MED} {AC.RESET}"],
				[$"{AC.FG_DARK_GREEN}{AC.BG_FOREST_LIGHT}▓{AC.RESET}", $"{AC.BG_FOREST_LIGHT} {AC.RESET}", $"{AC.BG_FOREST_LIGHT} {AC.RESET}", $"{AC.FG_MED_GREEN}{AC.BG_FOREST_LIGHT}♣{AC.RESET}", $"{AC.BG_FOREST_LIGHT} {AC.RESET}"],
				[$"{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.BG_FOREST_DARK} {AC.RESET}", $"{AC.BG_FOREST_DARK} {AC.RESET}"]
			]
		},
	};

	public static string getAnsiBackgroundColor(Color color) => color switch {
		Color.Red => AC.BG_STD_RED,
		Color.Gold => AC.BG_STD_GOLD,
		Color.Orange => AC.BG_STD_ORANGE,
		Color.Yellow => AC.BG_STD_YELLOW,
		Color.LightGreen => AC.BG_STD_LIGHT_GREEN,
		Color.DarkGreen => AC.BG_STD_DARK_GREEN,
		Color.Green => AC.BG_STD_GREEN,
		Color.LightBlue => AC.BG_STD_CYAN,
		Color.Blue => AC.BG_STD_BLUE,
		Color.Purple => AC.BG_STD_PURPLE,
		Color.White => AC.BG_STD_WHITE,
		Color.Gray => AC.BG_STD_GRAY,
		Color.Brown => AC.BG_STD_BROWN,
		_ => AC.RESET
	};

	public TerminalVersion(ISession<(uint, uint)> session, (int, int) map_size) {
		this.session = session;
		this.map_size = map_size;
		players = session.getAllPlayers();
	}

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

		// const string reset = "\u001b[0m";

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
									cell_line[i] = $"{p_color} {AC.RESET}";
								}
							}
						}
						if (yc == cell_size.y - 1) {
							if (!(y < map_size.y - 1) || cells[x][y + 1].ownership != current_owner) {
								for (int i = 0; i < cell_line.Length; i++) {
									cell_line[i] = $"{p_color} {AC.RESET}";
								}
							}
						}
						if (!(x > 0) || cells[x - 1][y].ownership != current_owner) {
							cell_line[0] = $"{p_color} {AC.RESET}";
						}
						if (!(x < map_size.x - 1) || cells[x + 1][y].ownership != current_owner) {
							cell_line[^1] = $"{p_color} {AC.RESET}";
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
			_ = sb.Append(CultureInfo.InvariantCulture, $"║{color}{content.PadRight(menuWidth)}{AC.RESET}║");
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
		clear();
		Console.WriteLine(res);
	}

	private static void clear() {
		Console.Write(new string('\n', Console.WindowHeight));
		Console.Write("\x1b[H");
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