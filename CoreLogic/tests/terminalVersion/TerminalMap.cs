global using TCell = CoreLogic.ICell<(uint x, uint y)>;

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
	private readonly Coord map_size;
	private readonly Func<PlayerKey, Color> getPlayerColor;
	private readonly Func<Coord, ErrorOr<TCell>> getCell;

	public TerminalMap(
		Coord map_size,
		Func<PlayerKey, Color> getPlayerColor,
		Func<Coord, ErrorOr<TCell>> getCell
	) {
		this.map_size = map_size;
		this.getPlayerColor = getPlayerColor;
		this.getCell = getCell;
	}

	private static void hightlightSingleCell(string[] cell_line, uint yc, string color) {
		string hightlight_str = $"{color}  {AC.RESET}";

		if (yc == 0 || yc == cell_size.y - 1) {
			for (int i = 0; i < cell_line.Length; i++) {
				cell_line[i] = hightlight_str;
			}
		}
		cell_line[0] = hightlight_str;
		cell_line[^1] = hightlight_str;
	}

	private static void hightlightMultipleCell(
		string[] cell_line,
		uint yc,
		string color,
		TCell cell,
		(TCell? top, TCell? bot, TCell? left, TCell? right) neighbours,
		Func<TCell, TCell?, bool> condition
	) {
		string hightlight_str = $"{color}  {AC.RESET}";

		if (yc == 0) {
			if (condition(cell, neighbours.top)) {
				for (int i = 0; i < cell_line.Length; i++) {
					cell_line[i] = hightlight_str;
				}
			}
		}
		if (yc == cell_size.y - 1) {
			if (condition(cell, neighbours.bot)) {
				for (int i = 0; i < cell_line.Length; i++) {
					cell_line[i] = hightlight_str;
				}
			}
		}
		if (condition(cell, neighbours.left)) {
			cell_line[0] = hightlight_str;
		}
		if (condition(cell, neighbours.right)) {
			cell_line[^1] = hightlight_str;
		}
	}

	public List<string> printDefaultMap(
		(Coord coord, string color, uint priority)[]? highlighted_coord = null
	) {
		Dictionary<Coord, string> highlighted_coord_set = highlighted_coord?
			.GroupBy(v => v.coord)
			.ToDictionary(
				g => g.Key,
				g => g.MaxBy(v => v.priority).color
			) ?? [];

		return printMap((cell, yc, neighbours) => {
			string[] cell_line = [.. backgrounds[cell.terrain][yc].Select(e => $"{e}{AC.RESET}")];

			if (highlighted_coord_set.TryGetValue(cell.id, out string? color)) {
				hightlightSingleCell(cell_line, yc, color);
			}
			else if (cell.owner.HasValue) {
				hightlightMultipleCell(
					cell_line,
					yc,
					getAnsiBackgroundColor(getPlayerColor(cell.owner.Value)),
					cell,
					neighbours,
					(cell, other) => other == null || other.owner != cell.owner
				);
			}
			return cell_line;
		});
	}

	public List<string> printPopMap(
		(Coord coord, string color, uint priority)[]? highlighted_coord = null
	) {
		Dictionary<Coord, string> highlighted_coord_set = highlighted_coord?
			.GroupBy(v => v.coord)
			.ToDictionary(
				g => g.Key,
				g => g.MaxBy(v => v.priority).color
			) ?? [];

		return printMap((cell, yc, neighbours) => {
			string[] cell_line = [.. Enumerable.Repeat("  ", cell_size.x)];

			if (highlighted_coord_set.TryGetValue(cell.id, out string? color)) {
				hightlightSingleCell(cell_line, yc, color);
			}
			else if (cell.owner.HasValue) {
				hightlightMultipleCell(
					cell_line,
					yc,
					getAnsiBackgroundColor(getPlayerColor(cell.owner.Value)),
					cell,
					neighbours,
					(cell, other) => other == null || other.owner != cell.owner
				);
			}
			return cell_line;
		});
	}

	private List<string> printMap(
		Func<TCell, uint, (TCell? top, TCell? bot, TCell? left, TCell? right), string[]> displayCell
	) {
		TCell[][] cells = new TCell[map_size.x][];
		for (uint x = 0; x < map_size.x; x++) {
			cells[x] = new TCell[map_size.y];
			for (uint y = 0; y < map_size.y; y++) {
				cells[x][y] = getCell((x + 1, y + 1)).Value;
			}
		}

		List<List<string[]>> map = [];
		for (uint y = 0; y < map_size.y; y++) {
			for (uint yc = 0; yc < cell_size.y; yc++) {
				List<string[]> line = [];
				for (uint x = 0; x < map_size.x; x++) {
					string[] cell_line = displayCell(
						cells[x][y],
						yc,
#pragma warning disable format
						(
							y > 0              ? cells[x][y - 1] : null,
							y < map_size.y - 1 ? cells[x][y + 1] : null,
							x > 0              ? cells[x - 1][y] : null,
							x < map_size.x - 1 ? cells[x + 1][y] : null
						)
#pragma warning restore format
					);

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