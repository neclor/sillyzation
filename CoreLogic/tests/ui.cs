using AC = AnsiColors;

internal interface IUserInterfaceTerminal {
	Pixel[,] display();
}

[System.Diagnostics.CodeAnalysis.SuppressMessage(
	"Performance",
	"CA1814:Prefer jagged arrays over multidimensional",
	Justification = "Structure is strictly rectangular by design"
)]
internal class Grid : IUserInterfaceTerminal {
	private readonly IUserInterfaceTerminal[] components;
	private readonly char[,] layoutMap;
	private readonly int lx;
	private readonly int ly;

	public Grid(
		int[,] layoutMap,
		IUserInterfaceTerminal[] components
	) {
		this.components = components;

		this.layoutMap = transformToVisualGrid(layoutMap);
		lx = this.layoutMap.GetLength(0);
		ly = this.layoutMap.GetLength(1);

		string res = "";
		for (int y = 0; y < ly; y++) {
			for (int x = 0; x < lx; x++) {
				res += this.layoutMap[x, y];
			}
			res += "\n";
		}
		Console.WriteLine(res);
	}

	private static char[,] transformToVisualGrid(int[,] map) {
		int rows = map.GetLength(0);
		int cols = map.GetLength(1);

		int targetRows = (rows * 2) + 1;
		int targetCols = (cols * 2) + 1;
		char[,] result = new char[targetCols, targetRows];

		for (int c = 0; c < targetCols; c++) {
			for (int r = 0; r < targetRows; r++) {
				bool isRowEven = r % 2 == 0;
				bool isColEven = c % 2 == 0;

				if (isRowEven && isColEven) {
					result[c, r] = getIntersectionChar(map, r / 2, c / 2);
				}
				else if (isRowEven && !isColEven) {
					result[c, r] = getHorizontalChar(map, r / 2, c / 2);
				}
				else if (!isRowEven && isColEven) {
					result[c, r] = getVerticalChar(map, r / 2, c / 2);
				}
				else {
					result[c, r] = (char) (map[r / 2, c / 2] + '0');
				}
			}
		}

		return result;
	}

	private static bool isValid(int[,] map, int r, int c) {
		return r >= 0 && r < map.GetLength(0) && c >= 0 && c < map.GetLength(1);
	}

	private static char getVal(int[,] map, int r, int c) {
		return isValid(map, r, c) ? (char) (map[r, c] + '0') : '\0';
	}

	private static char getHorizontalChar(int[,] map, int r, int c) {
		char top = getVal(map, r - 1, c);
		char bottom = getVal(map, r, c);
		return (top == bottom) ? top : '═';
	}

	private static char getVerticalChar(int[,] map, int r, int c) {
		char left = getVal(map, r, c - 1);
		char right = getVal(map, r, c);
		return (left == right) ? left : '║';
	}

	private static char getIntersectionChar(int[,] map, int r, int c) {
		int tL = getVal(map, r - 1, c - 1);
		int tR = getVal(map, r - 1, c);
		int bL = getVal(map, r, c - 1);
		int bR = getVal(map, r, c);

		bool up = (tL != tR) && (tL != '\0' || tR != '\0');
		bool down = (bL != bR) && (bL != '\0' || bR != '\0');
		bool left = (tL != bL) && (tL != '\0' || bL != '\0');
		bool right = (tR != bR) && (tR != '\0' || bR != '\0');

		if (r == 0) { left = c > 0; right = c < map.GetLength(1); }
		if (r == map.GetLength(0)) { left = c > 0; right = c < map.GetLength(1); }
		if (c == 0) { up = r > 0; down = r < map.GetLength(0); }
		if (c == map.GetLength(1)) { up = r > 0; down = r < map.GetLength(0); }

		if (up && down && left && right)
			return '╬';
		if (up && down && left)
			return '╣';
		if (up && down && right)
			return '╠';
		if (up && left && right)
			return '╩';
		if (down && left && right)
			return '╦';
		if (up && left)
			return '╝';
		if (up && right)
			return '╚';
		if (down && left)
			return '╗';
		if (down && right)
			return '╔';
		if (up || down)
			return '║';
		if (left || right)
			return '═';

		return ' ';
	}

	public Pixel[,] display() {
		Pixel[][,] componentsRes = [.. components.Select(e => e.display())];

		int layoutWidth = layoutMap.GetLength(0);  // lx (X dimension / Rows)
		int layoutHeight = layoutMap.GetLength(1); // ly (Y dimension / Cols)

		// Even indices are ALWAYS borders/lines in the generated visual grid
		bool[] isBorderCol = new bool[layoutWidth];
		for (int x = 0; x < layoutWidth; x++) {
			isBorderCol[x] = (x % 2 == 0);
		}

		bool[] isBorderRow = new bool[layoutHeight];
		for (int y = 0; y < layoutHeight; y++) {
			isBorderRow[y] = (y % 2 == 0);
		}

		int[] colWidths = new int[layoutWidth];
		int[] rowHeights = new int[layoutHeight];

		for (int x = 0; x < layoutWidth; x++)
			colWidths[x] = isBorderCol[x] ? 1 : 0;
		for (int y = 0; y < layoutHeight; y++)
			rowHeights[y] = isBorderRow[y] ? 1 : 0;

		// Calculate track expansion constraints
		for (int i = 0; i < componentsRes.Length; i++) {
			char id = i.ToString()[0];
			int compWidth = componentsRes[i].GetLength(0);
			int compHeight = componentsRes[i].GetLength(1);

			int minX = int.MaxValue, maxX = int.MinValue;
			int minY = int.MaxValue, maxY = int.MinValue;
			bool found = false;

			for (int x = 0; x < layoutWidth; x++) {
				for (int y = 0; y < layoutHeight; y++) {
					if (layoutMap[x, y] == id) {
						if (x < minX)
							minX = x;
						if (x > maxX)
							maxX = x;
						if (y < minY)
							minY = y;
						if (y > maxY)
							maxY = y;
						found = true;
					}
				}
			}

			if (!found)
				continue;

			int contentSpanX = 0;
			for (int x = minX; x <= maxX; x++) {
				if (!isBorderCol[x])
					contentSpanX++;
			}

			int contentSpanY = 0;
			for (int y = minY; y <= maxY; y++) {
				if (!isBorderRow[y])
					contentSpanY++;
			}

			int reqWidthPerCell = (int) Math.Ceiling((double) (compWidth + 2) / (contentSpanX > 0 ? contentSpanX : 1));
			int reqHeightPerCell = (int) Math.Ceiling((double) compHeight / (contentSpanY > 0 ? contentSpanY : 1));

			for (int x = minX; x <= maxX; x++) {
				if (!isBorderCol[x] && reqWidthPerCell > colWidths[x])
					colWidths[x] = reqWidthPerCell;
			}

			for (int y = minY; y <= maxY; y++) {
				if (!isBorderRow[y] && reqHeightPerCell > rowHeights[y])
					rowHeights[y] = reqHeightPerCell;
			}
		}

		int totalWidth = colWidths.Sum();
		int totalHeight = rowHeights.Sum();
		Pixel[,] result = new Pixel[totalWidth, totalHeight];

		int[] colOffsets = new int[layoutWidth];
		int[] rowOffsets = new int[layoutHeight];
		for (int x = 1; x < layoutWidth; x++)
			colOffsets[x] = colOffsets[x - 1] + colWidths[x - 1];
		for (int y = 1; y < layoutHeight; y++)
			rowOffsets[y] = rowOffsets[y - 1] + rowHeights[y - 1];

		// Render elements directly onto the canvas
		for (int x = 0; x < layoutWidth; x++) {
			for (int y = 0; y < layoutHeight; y++) {
				char cell = layoutMap[x, y];
				int startX = colOffsets[x];
				int startY = rowOffsets[y];
				int cellW = colWidths[x];
				int cellH = rowHeights[y];

				if (char.IsDigit(cell)) {
					int compIdx = cell - '0';
					var comp = componentsRes[compIdx];

					int blockStartX = int.MaxValue;
					int blockStartY = int.MaxValue;
					for (int i = 0; i < layoutWidth; i++) {
						for (int j = 0; j < layoutHeight; j++) {
							if (layoutMap[i, j] == cell) {
								if (colOffsets[i] < blockStartX)
									blockStartX = colOffsets[i];
								if (rowOffsets[j] < blockStartY)
									blockStartY = rowOffsets[j];
							}
						}
					}

					for (int dx = 0; dx < cellW; dx++) {
						for (int dy = 0; dy < cellH; dy++) {
							int absX = startX + dx;
							int absY = startY + dy;

							int compX = 0;
							for (int i = blockStartX; i < absX; i++) {
								int layX = Array.FindIndex(colOffsets, v => v > i);
								if (layX == -1)
									layX = layoutWidth;
								layX -= 1;
								if (!isBorderCol[layX])
									compX++;
							}

							int compY = 0;
							for (int j = blockStartY; j < absY; j++) {
								int layY = Array.FindIndex(rowOffsets, v => v > j);
								if (layY == -1)
									layY = layoutHeight;
								layY -= 1;
								if (!isBorderRow[layY])
									compY++;
							}

							int sourceX = compX - 1;
							if (sourceX >= 0 && sourceX < comp.GetLength(0) && compY < comp.GetLength(1)) {
								result[absX, absY] = comp[sourceX, compY];
							}
							else {
								result[absX, absY] = new Pixel(' ');
							}
						}
					}
				}
				else {
					for (int dx = 0; dx < cellW; dx++) {
						for (int dy = 0; dy < cellH; dy++) {
							result[startX + dx, startY + dy] = new Pixel(cell);
						}
					}
				}
			}
		}

		return result;
	}
}

internal class TopBar : IUserInterfaceTerminal {
	private readonly Func<(string country, AC country_color, string info)> get_current_info;

	public TopBar(Func<(string country, AC country_color, string info)> get_current_info) {
		this.get_current_info = get_current_info;
	}

	public Pixel[,] display() {
		(string country, AC country_color, string info) = get_current_info();

		string to_print = $"Country: {country} {info}";

		Pixel[,] result = new Pixel[to_print.Length, 1];
		foreach ((int i, char c) in to_print.Index()) {
			result[i, 0] = new Pixel(c);
		}

		return result;
	}
}

internal class Menu : IUserInterfaceTerminal {
	private static readonly AC highlight_color = AC.STD_GOLD;
	private (string content, bool highlighted)[] contents = [];

	public void setContent((string content, bool highlighted)[] contents) {
		this.contents = contents;
	}

	public Pixel[,] display() {
		int longest = contents.Max((c) => c.content.Length);

		Pixel[,] res = new Pixel[longest, contents.Length];

		foreach ((int index, (string content, bool highlighted)) in contents.Index()) {
			AC color = highlighted ? highlight_color : AC.RESET;
			foreach ((int i, char c) in content.Index()) {
				res[i, index] = new Pixel(c, color);
			}
			for (int i = content.Length; i < longest; i++) {
				res[i, index] = new Pixel(color);
			}
		}
		return res;
	}
}
