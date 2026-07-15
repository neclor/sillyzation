using AC = AnsiColors;
using ErrorOr;
using CoreLogic;
using Neighbours = (
	CoreLogic.ICell<(uint x, uint y)>? top,
	CoreLogic.ICell<(uint x, uint y)>? bot,
	CoreLogic.ICell<(uint x, uint y)>? left,
	CoreLogic.ICell<(uint x, uint y)>? right
);
using Highlights = (
	AnsiColors? top,
	AnsiColors? bot,
	AnsiColors? left,
	AnsiColors? right
);
using System.Text;

internal abstract class CellTexture {
	public abstract AC value(uint x, uint y);
}

internal class BlockCellTexture : CellTexture {
	private readonly AC[][] texture;
	public BlockCellTexture(AC[][] texture) => this.texture = texture;
	public override AC value(uint x, uint y) => texture[y][x];
}

internal class UniformCellTexture : CellTexture {
	private readonly AC color;
	public UniformCellTexture(AC color) => this.color = color;
	public override AC value(uint x, uint y) => color;
}

internal class TerminalMap : IUserInterfaceTerminal {
	private static readonly (int x, int y) cell_size = (10, 4);
	private readonly Coord map_size;
	private readonly Func<Coord, ErrorOr<TCell>> get_cell;
	private readonly Func<TCell, CellTexture> get_cell_texture;
	private readonly Func<TCell, Neighbours, Highlights> get_cell_highlight;
	private static readonly Dictionary<Terrain, CellTexture> backgrounds = new() {
		{
			Terrain.Plain, new BlockCellTexture([
				[AC.PLAIN_1, AC.PLAIN_1, AC.PLAIN_2, AC.PLAIN_2, AC.PLAIN_3, AC.PLAIN_3, AC.PLAIN_4, AC.PLAIN_4, AC.PLAIN_1, AC.PLAIN_1],
				[AC.PLAIN_4, AC.PLAIN_4, AC.PLAIN_3, AC.PLAIN_3, AC.PLAIN_2, AC.PLAIN_2, AC.PLAIN_1, AC.PLAIN_1, AC.PLAIN_2, AC.PLAIN_2],
				[AC.PLAIN_2, AC.PLAIN_2, AC.PLAIN_1, AC.PLAIN_1, AC.PLAIN_4, AC.PLAIN_4, AC.PLAIN_3, AC.PLAIN_3, AC.PLAIN_4, AC.PLAIN_4],
				[AC.PLAIN_3, AC.PLAIN_3, AC.PLAIN_4, AC.PLAIN_4, AC.PLAIN_1, AC.PLAIN_1, AC.PLAIN_2, AC.PLAIN_2, AC.PLAIN_3, AC.PLAIN_3]
			])
		},
		{
			Terrain.Swamp, new BlockCellTexture([
				[AC.SWAMP_1, AC.SWAMP_1, AC.SWAMP_2, AC.SWAMP_2, AC.SWAMP_3, AC.SWAMP_3, AC.SWAMP_4, AC.SWAMP_4, AC.SWAMP_1, AC.SWAMP_1],
				[AC.SWAMP_4, AC.SWAMP_4, AC.SWAMP_3, AC.SWAMP_3, AC.SWAMP_2, AC.SWAMP_2, AC.SWAMP_1, AC.SWAMP_1, AC.SWAMP_2, AC.SWAMP_2],
				[AC.SWAMP_2, AC.SWAMP_2, AC.SWAMP_1, AC.SWAMP_1, AC.SWAMP_4, AC.SWAMP_4, AC.SWAMP_3, AC.SWAMP_3, AC.SWAMP_4, AC.SWAMP_4],
				[AC.SWAMP_3, AC.SWAMP_3, AC.SWAMP_4, AC.SWAMP_4, AC.SWAMP_1, AC.SWAMP_1, AC.SWAMP_2, AC.SWAMP_2, AC.SWAMP_3, AC.SWAMP_3]
			])
		},
		{
			Terrain.Forest, new BlockCellTexture([
				[AC.FOREST_1, AC.FOREST_1, AC.FOREST_2, AC.FOREST_2, AC.FOREST_3, AC.FOREST_3, AC.FOREST_4, AC.FOREST_4, AC.FOREST_1, AC.FOREST_1],
				[AC.FOREST_4, AC.FOREST_4, AC.FOREST_3, AC.FOREST_3, AC.FOREST_2, AC.FOREST_2, AC.FOREST_1, AC.FOREST_1, AC.FOREST_2, AC.FOREST_2],
				[AC.FOREST_2, AC.FOREST_2, AC.FOREST_1, AC.FOREST_1, AC.FOREST_4, AC.FOREST_4, AC.FOREST_3, AC.FOREST_3, AC.FOREST_4, AC.FOREST_4],
				[AC.FOREST_3, AC.FOREST_3, AC.FOREST_4, AC.FOREST_4, AC.FOREST_1, AC.FOREST_1, AC.FOREST_2, AC.FOREST_2, AC.FOREST_3, AC.FOREST_3]
			])
		},
		{
			Terrain.Desert, new BlockCellTexture([
				[AC.DESERT_1, AC.DESERT_1, AC.DESERT_2, AC.DESERT_2, AC.DESERT_3, AC.DESERT_3, AC.DESERT_4, AC.DESERT_4, AC.DESERT_1, AC.DESERT_1],
				[AC.DESERT_4, AC.DESERT_4, AC.DESERT_3, AC.DESERT_3, AC.DESERT_2, AC.DESERT_2, AC.DESERT_1, AC.DESERT_1, AC.DESERT_2, AC.DESERT_2],
				[AC.DESERT_2, AC.DESERT_2, AC.DESERT_1, AC.DESERT_1, AC.DESERT_4, AC.DESERT_4, AC.DESERT_3, AC.DESERT_3, AC.DESERT_4, AC.DESERT_4],
				[AC.DESERT_3, AC.DESERT_3, AC.DESERT_4, AC.DESERT_4, AC.DESERT_1, AC.DESERT_1, AC.DESERT_2, AC.DESERT_2, AC.DESERT_3, AC.DESERT_3]
			])
		},
		{
			Terrain.Tundra, new BlockCellTexture([
				[AC.TUNDRA_1, AC.TUNDRA_1, AC.TUNDRA_2, AC.TUNDRA_2, AC.TUNDRA_3, AC.TUNDRA_3, AC.TUNDRA_4, AC.TUNDRA_4, AC.TUNDRA_1, AC.TUNDRA_1],
				[AC.TUNDRA_4, AC.TUNDRA_4, AC.TUNDRA_3, AC.TUNDRA_3, AC.TUNDRA_2, AC.TUNDRA_2, AC.TUNDRA_1, AC.TUNDRA_1, AC.TUNDRA_2, AC.TUNDRA_2],
				[AC.TUNDRA_2, AC.TUNDRA_2, AC.TUNDRA_1, AC.TUNDRA_1, AC.TUNDRA_4, AC.TUNDRA_4, AC.TUNDRA_3, AC.TUNDRA_3, AC.TUNDRA_4, AC.TUNDRA_4],
				[AC.TUNDRA_3, AC.TUNDRA_3, AC.TUNDRA_4, AC.TUNDRA_4, AC.TUNDRA_1, AC.TUNDRA_1, AC.TUNDRA_2, AC.TUNDRA_2, AC.TUNDRA_3, AC.TUNDRA_3]
			])
		},
		{
			Terrain.Savanna, new BlockCellTexture([
				[AC.SAVANNA_1, AC.SAVANNA_1, AC.SAVANNA_2, AC.SAVANNA_2, AC.SAVANNA_3, AC.SAVANNA_3, AC.SAVANNA_4, AC.SAVANNA_4, AC.SAVANNA_1, AC.SAVANNA_1],
				[AC.SAVANNA_4, AC.SAVANNA_4, AC.SAVANNA_3, AC.SAVANNA_3, AC.SAVANNA_2, AC.SAVANNA_2, AC.SAVANNA_1, AC.SAVANNA_1, AC.SAVANNA_2, AC.SAVANNA_2],
				[AC.SAVANNA_2, AC.SAVANNA_2, AC.SAVANNA_1, AC.SAVANNA_1, AC.SAVANNA_4, AC.SAVANNA_4, AC.SAVANNA_3, AC.SAVANNA_3, AC.SAVANNA_4, AC.SAVANNA_4],
				[AC.SAVANNA_3, AC.SAVANNA_3, AC.SAVANNA_4, AC.SAVANNA_4, AC.SAVANNA_1, AC.SAVANNA_1, AC.SAVANNA_2, AC.SAVANNA_2, AC.SAVANNA_3, AC.SAVANNA_3]
			])
		},
		{
			Terrain.Jungle, new BlockCellTexture([
				[AC.JUNGLE_1, AC.JUNGLE_1, AC.JUNGLE_2, AC.JUNGLE_2, AC.JUNGLE_3, AC.JUNGLE_3, AC.JUNGLE_4, AC.JUNGLE_4, AC.JUNGLE_1, AC.JUNGLE_1],
				[AC.JUNGLE_4, AC.JUNGLE_4, AC.JUNGLE_3, AC.JUNGLE_3, AC.JUNGLE_2, AC.JUNGLE_2, AC.JUNGLE_1, AC.JUNGLE_1, AC.JUNGLE_2, AC.JUNGLE_2],
				[AC.JUNGLE_2, AC.JUNGLE_2, AC.JUNGLE_1, AC.JUNGLE_1, AC.JUNGLE_4, AC.JUNGLE_4, AC.JUNGLE_3, AC.JUNGLE_3, AC.JUNGLE_4, AC.JUNGLE_4],
				[AC.JUNGLE_3, AC.JUNGLE_3, AC.JUNGLE_4, AC.JUNGLE_4, AC.JUNGLE_1, AC.JUNGLE_1, AC.JUNGLE_2, AC.JUNGLE_2, AC.JUNGLE_3, AC.JUNGLE_3]
			])
		},
	};

	public static CellTexture getTerrainTexture(Terrain terrain) {
		return backgrounds[terrain];
	}

	//  ▄███▬
	// (⯄⯄⯄⯄)

	public TerminalMap(
		Coord map_size,
		Func<Coord, ErrorOr<TCell>> get_cell,
		Func<TCell, CellTexture> get_cell_texture,
		Func<TCell, Neighbours, Highlights> get_cell_highlight
	) {
		this.map_size = map_size;
		this.get_cell_texture = get_cell_texture;
		this.get_cell_highlight = get_cell_highlight;
		this.get_cell = get_cell;
	}

	public Pixel[,] display() {
		TCell[,] cells = new TCell[map_size.x, map_size.y];
		for (uint x = 0; x < map_size.x; x++) {
			for (uint y = 0; y < map_size.y; y++) {
				cells[x, y] = get_cell((x + 1, y + 1)).Value;
			}
		}

		Pixel[,] res = new Pixel[
			map_size.x * cell_size.x,
			map_size.y * cell_size.y
		];

		for (uint x = 0; x < map_size.x; x++) {
			for (uint y = 0; y < map_size.y; y++) {
				TCell cell = cells[x, y];
				Neighbours neighbours = (
					y > 0 ? cells[x, y - 1] : null,
					y < map_size.y - 1 ? cells[x, y + 1] : null,
					x > 0 ? cells[x - 1, y] : null,
					x < map_size.x - 1 ? cells[x + 1, y] : null
				);
				CellTexture cell_texture = get_cell_texture(cell);
				for (uint xc = 0; xc < cell_size.x; xc++) {
					for (uint yc = 0; yc < cell_size.y; yc++) {
						res[
							(x * cell_size.x) + xc,
							(y * cell_size.y) + yc
						] = new Pixel(cell_texture.value(xc, yc));
					}
				}
				Highlights highlights = get_cell_highlight(cell, neighbours);
				if (highlights.top != null) {
					for (uint xc = 0; xc < cell_size.x; xc++) {
						res[
							(x * cell_size.x) + xc,
							y * cell_size.y
						] = new Pixel(highlights.top);
					}
				}
				if (highlights.bot != null) {
					for (uint xc = 0; xc < cell_size.x; xc++) {
						res[
							(x * cell_size.x) + xc,
							(y * cell_size.y) + cell_size.y - 1
						] = new Pixel(highlights.bot);
					}
				}
				if (highlights.left != null) {
					for (uint yc = 0; yc < cell_size.y; yc++) {
						res[
							x * cell_size.x,
							(y * cell_size.y) + yc
						] = new Pixel(highlights.left);
					}
				}
				if (highlights.right != null) {
					for (uint yc = 0; yc < cell_size.y; yc++) {
						res[
							(x * cell_size.x) + cell_size.x - 1,
							(y * cell_size.y) + yc
						] = new Pixel(highlights.right);
					}
				}
			}
		}
		return res;
	}

	public static string getAnsiBackgroundColor(Color color) => color switch {
		Color.Red => AC.STD_RED.bg(),
		Color.Gold => AC.STD_GOLD.bg(),
		Color.Orange => AC.STD_ORANGE.bg(),
		Color.Yellow => AC.STD_YELLOW.bg(),
		Color.LightGreen => AC.STD_LIGHT_GREEN.bg(),
		Color.DarkGreen => AC.STD_DARK_GREEN.bg(),
		Color.Green => AC.STD_GREEN.bg(),
		Color.LightBlue => AC.STD_CYAN.bg(),
		Color.Blue => AC.STD_BLUE.bg(),
		Color.Purple => AC.STD_PURPLE.bg(),
		Color.White => AC.STD_WHITE.bg(),
		Color.Gray => AC.STD_GRAY.bg(),
		Color.Brown => AC.STD_BROWN.bg(),
		_ => AC.RESET.bg()
	};
}
