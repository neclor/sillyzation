using AC = AnsiColors;
using ErrorOr;
using CoreLogic;

internal class TerminalMap {
	private static readonly (int x, int y) cell_size = (5, 4);
	private static readonly Dictionary<Terrain, string[][]> backgrounds = new() {
		{
			Terrain.Plain, [
				[$"{AC.BG_PLAIN_1}  ", $"{AC.BG_PLAIN_2}  ", $"{AC.BG_PLAIN_3}  ", $"{AC.BG_PLAIN_4}  ", $"{AC.BG_PLAIN_1}  "],
				[$"{AC.BG_PLAIN_4}  ", $"{AC.BG_PLAIN_3}  ", $"{AC.BG_PLAIN_2}  ", $"{AC.BG_PLAIN_1}  ", $"{AC.BG_PLAIN_2}  "],
				[$"{AC.BG_PLAIN_2}  ", $"{AC.BG_PLAIN_1}  ", $"{AC.BG_PLAIN_4}  ", $"{AC.BG_PLAIN_3}  ", $"{AC.BG_PLAIN_4}  "],
				[$"{AC.BG_PLAIN_3}  ", $"{AC.BG_PLAIN_4}  ", $"{AC.BG_PLAIN_1}  ", $"{AC.BG_PLAIN_2}  ", $"{AC.BG_PLAIN_3}  "]
			]
		},
		{
			Terrain.Swamp, [
				[$"{AC.BG_SWAMP_1}  ", $"{AC.BG_SWAMP_2}  ", $"{AC.BG_SWAMP_3}  ", $"{AC.BG_SWAMP_4}  ", $"{AC.BG_SWAMP_1}  "],
				[$"{AC.BG_SWAMP_4}  ", $"{AC.BG_SWAMP_3}  ", $"{AC.BG_SWAMP_2}  ", $"{AC.BG_SWAMP_1}  ", $"{AC.BG_SWAMP_2}  "],
				[$"{AC.BG_SWAMP_2}  ", $"{AC.BG_SWAMP_1}  ", $"{AC.BG_SWAMP_4}  ", $"{AC.BG_SWAMP_3}  ", $"{AC.BG_SWAMP_4}  "],
				[$"{AC.BG_SWAMP_3}  ", $"{AC.BG_SWAMP_4}  ", $"{AC.BG_SWAMP_1}  ", $"{AC.BG_SWAMP_2}  ", $"{AC.BG_SWAMP_3}  "]
			]
		},
		{
			Terrain.Forest, [
				[$"{AC.BG_FOREST_1}  ", $"{AC.BG_FOREST_2}  ", $"{AC.BG_FOREST_3}  ", $"{AC.BG_FOREST_4}  ", $"{AC.BG_FOREST_1}  "],
				[$"{AC.BG_FOREST_4}  ", $"{AC.BG_FOREST_3}  ", $"{AC.BG_FOREST_2}  ", $"{AC.BG_FOREST_1}  ", $"{AC.BG_FOREST_2}  "],
				[$"{AC.BG_FOREST_2}  ", $"{AC.BG_FOREST_1}  ", $"{AC.BG_FOREST_4}  ", $"{AC.BG_FOREST_3}  ", $"{AC.BG_FOREST_4}  "],
				[$"{AC.BG_FOREST_3}  ", $"{AC.BG_FOREST_4}  ", $"{AC.BG_FOREST_1}  ", $"{AC.BG_FOREST_2}  ", $"{AC.BG_FOREST_3}  "]
			]
		},
		{
			Terrain.Desert, [
				[$"{AC.BG_DESERT_1}  ", $"{AC.BG_DESERT_2}  ", $"{AC.BG_DESERT_3}  ", $"{AC.BG_DESERT_4}  ", $"{AC.BG_DESERT_1}  "],
				[$"{AC.BG_DESERT_4}  ", $"{AC.BG_DESERT_3}  ", $"{AC.BG_DESERT_2}  ", $"{AC.BG_DESERT_1}  ", $"{AC.BG_DESERT_2}  "],
				[$"{AC.BG_DESERT_2}  ", $"{AC.BG_DESERT_1}  ", $"{AC.BG_DESERT_4}  ", $"{AC.BG_DESERT_3}  ", $"{AC.BG_DESERT_4}  "],
				[$"{AC.BG_DESERT_3}  ", $"{AC.BG_DESERT_4}  ", $"{AC.BG_DESERT_1}  ", $"{AC.BG_DESERT_2}  ", $"{AC.BG_DESERT_3}  "]
			]
		},
		{
			Terrain.Tundra, [
				[$"{AC.BG_TUNDRA_1}  ", $"{AC.BG_TUNDRA_2}  ", $"{AC.BG_TUNDRA_3}  ", $"{AC.BG_TUNDRA_4}  ", $"{AC.BG_TUNDRA_1}  "],
				[$"{AC.BG_TUNDRA_4}  ", $"{AC.BG_TUNDRA_3}  ", $"{AC.BG_TUNDRA_2}  ", $"{AC.BG_TUNDRA_1}  ", $"{AC.BG_TUNDRA_2}  "],
				[$"{AC.BG_TUNDRA_2}  ", $"{AC.BG_TUNDRA_1}  ", $"{AC.BG_TUNDRA_4}  ", $"{AC.BG_TUNDRA_3}  ", $"{AC.BG_TUNDRA_4}  "],
				[$"{AC.BG_TUNDRA_3}  ", $"{AC.BG_TUNDRA_4}  ", $"{AC.BG_TUNDRA_1}  ", $"{AC.BG_TUNDRA_2}  ", $"{AC.BG_TUNDRA_3}  "]
			]
		},
		{
			Terrain.Savanna, [
				[$"{AC.BG_SAVANNA_1}  ", $"{AC.BG_SAVANNA_2}  ", $"{AC.BG_SAVANNA_3}  ", $"{AC.BG_SAVANNA_4}  ", $"{AC.BG_SAVANNA_1}  "],
				[$"{AC.BG_SAVANNA_4}  ", $"{AC.BG_SAVANNA_3}  ", $"{AC.BG_SAVANNA_2}  ", $"{AC.BG_SAVANNA_1}  ", $"{AC.BG_SAVANNA_2}  "],
				[$"{AC.BG_SAVANNA_2}  ", $"{AC.BG_SAVANNA_1}  ", $"{AC.BG_SAVANNA_4}  ", $"{AC.BG_SAVANNA_3}  ", $"{AC.BG_SAVANNA_4}  "],
				[$"{AC.BG_SAVANNA_3}  ", $"{AC.BG_SAVANNA_4}  ", $"{AC.BG_SAVANNA_1}  ", $"{AC.BG_SAVANNA_2}  ", $"{AC.BG_SAVANNA_3}  "]
			]
		},
		{
			Terrain.Jungle, [
				[$"{AC.BG_JUNGLE_1}  ", $"{AC.BG_JUNGLE_2}  ", $"{AC.BG_JUNGLE_3}  ", $"{AC.BG_JUNGLE_4}  ", $"{AC.BG_JUNGLE_1}  "],
				[$"{AC.BG_JUNGLE_4}  ", $"{AC.BG_JUNGLE_3}  ", $"{AC.BG_JUNGLE_2}  ", $"{AC.BG_JUNGLE_1}  ", $"{AC.BG_JUNGLE_2}  "],
				[$"{AC.BG_JUNGLE_2}  ", $"{AC.BG_JUNGLE_1}  ", $"{AC.BG_JUNGLE_4}  ", $"{AC.BG_JUNGLE_3}  ", $"{AC.BG_JUNGLE_4}  "],
				[$"{AC.BG_JUNGLE_3}  ", $"{AC.BG_JUNGLE_4}  ", $"{AC.BG_JUNGLE_1}  ", $"{AC.BG_JUNGLE_2}  ", $"{AC.BG_JUNGLE_3}  "]
			]
		},
	};

	public static List<string> printMap(
		(int x, int y) map_size,
		Func<uint, Color> getPlayerColor,
		Func<(uint x, uint y), ErrorOr<ICell<(uint, uint)>>> getCell
	) {
		var cells = new (Terrain terrain, uint? ownership)[map_size.x][];
		for (uint x = 0; x < map_size.x; x++) {
			cells[x] = new (Terrain terrain, uint? ownership)[map_size.y];
			for (uint y = 0; y < map_size.y; y++) {
				ErrorOr<ICell<(uint, uint)>> cell = getCell((x + 1, y + 1));
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
					string[] cell_line = [.. backgrounds[cells[x][y].terrain][yc].Select(e => $"{e}{AC.RESET}")];
					uint? current_owner = cells[x][y].ownership;

					if (current_owner.HasValue) {
						string p_color = getAnsiBackgroundColor(getPlayerColor(current_owner.Value));
						if (yc == 0) {
							if (!(y > 0) || cells[x][y - 1].ownership != current_owner) {
								for (int i = 0; i < cell_line.Length; i++) {
									cell_line[i] = $"{p_color}  {AC.RESET}";
								}
							}
						}
						if (yc == cell_size.y - 1) {
							if (!(y < map_size.y - 1) || cells[x][y + 1].ownership != current_owner) {
								for (int i = 0; i < cell_line.Length; i++) {
									cell_line[i] = $"{p_color}  {AC.RESET}";
								}
							}
						}
						if (!(x > 0) || cells[x - 1][y].ownership != current_owner) {
							cell_line[0] = $"{p_color}  {AC.RESET}";
						}
						if (!(x < map_size.x - 1) || cells[x + 1][y].ownership != current_owner) {
							cell_line[^1] = $"{p_color}  {AC.RESET}";
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
}