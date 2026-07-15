internal abstract class AnsiColors {
	public abstract string bg();
	public abstract string fg();

	public static readonly AnsiColors RESET = new ColReset();

	public static readonly AnsiColors STD_RED = new ColId(196);
	public static readonly AnsiColors STD_GOLD = new ColId(220);
	public static readonly AnsiColors STD_ORANGE = new ColId(208);
	public static readonly AnsiColors STD_YELLOW = new ColId(226);
	public static readonly AnsiColors STD_LIGHT_GREEN = new ColId(119);
	public static readonly AnsiColors STD_DARK_GREEN = new ColId(40);
	public static readonly AnsiColors STD_GREEN = new ColId(46);
	public static readonly AnsiColors STD_CYAN = new ColId(87);
	public static readonly AnsiColors STD_BLUE = new ColId(27);
	public static readonly AnsiColors STD_PURPLE = new ColId(201);
	public static readonly AnsiColors STD_WHITE = new ColId(255);
	public static readonly AnsiColors STD_GRAY = new ColId(250);
	public static readonly AnsiColors STD_BROWN = new ColId(172);

	// Plain
	public static readonly AnsiColors PLAIN_1 = new ColTrue(35, 48, 35);
	public static readonly AnsiColors PLAIN_2 = new ColTrue(30, 42, 30);
	public static readonly AnsiColors PLAIN_3 = new ColTrue(40, 54, 40);
	public static readonly AnsiColors PLAIN_4 = new ColTrue(25, 36, 25);

	// Forest
	public static readonly AnsiColors FOREST_1 = new ColTrue(15, 45, 18);
	public static readonly AnsiColors FOREST_2 = new ColTrue(12, 38, 15);
	public static readonly AnsiColors FOREST_3 = new ColTrue(18, 52, 21);
	public static readonly AnsiColors FOREST_4 = new ColTrue(10, 32, 12);

	// Desert
	public static readonly AnsiColors DESERT_1 = new ColTrue(145, 85, 35);
	public static readonly AnsiColors DESERT_2 = new ColTrue(135, 78, 30);
	public static readonly AnsiColors DESERT_3 = new ColTrue(155, 92, 40);
	public static readonly AnsiColors DESERT_4 = new ColTrue(125, 72, 26);

	// Tundra
	public static readonly AnsiColors TUNDRA_1 = new ColTrue(34, 38, 44);
	public static readonly AnsiColors TUNDRA_2 = new ColTrue(30, 34, 40);
	public static readonly AnsiColors TUNDRA_3 = new ColTrue(38, 42, 48);
	public static readonly AnsiColors TUNDRA_4 = new ColTrue(26, 29, 34);

	// Savanna
	public static readonly AnsiColors SAVANNA_1 = new ColTrue(115, 85, 25);
	public static readonly AnsiColors SAVANNA_2 = new ColTrue(105, 77, 20);
	public static readonly AnsiColors SAVANNA_3 = new ColTrue(125, 93, 30);
	public static readonly AnsiColors SAVANNA_4 = new ColTrue(95, 69, 16);

	// Swamp
	public static readonly AnsiColors SWAMP_1 = new ColTrue(18, 40, 38);
	public static readonly AnsiColors SWAMP_2 = new ColTrue(15, 34, 32);
	public static readonly AnsiColors SWAMP_3 = new ColTrue(22, 46, 44);
	public static readonly AnsiColors SWAMP_4 = new ColTrue(12, 28, 26);

	// Jungle
	public static readonly AnsiColors JUNGLE_1 = new ColTrue(32, 72, 36);
	public static readonly AnsiColors JUNGLE_2 = new ColTrue(24, 54, 30);
	public static readonly AnsiColors JUNGLE_3 = new ColTrue(40, 90, 42);
	public static readonly AnsiColors JUNGLE_4 = new ColTrue(16, 36, 24);
}

internal class ColReset : AnsiColors {
	public override string bg() => "\x1b[49m";
	public override string fg() => "\x1b[39m";
}

internal class ColId : AnsiColors {
	private readonly uint color_id;
	public ColId(uint color_id) => this.color_id = color_id;
	public override string bg() => $"\x1b[48;5;{color_id}m";
	public override string fg() => $"\x1b[38;5;{color_id}m";
}

internal class ColTrue : AnsiColors {
	private readonly (char r, char g, char b) color;
	public ColTrue(uint r, uint g, uint b) => color = ((char) r, (char) g, (char) b);
	public ColTrue(char r, char g, char b) => color = (r, g, b);
	public override string bg() => $"\x1b[48;2;{(int) color.r};{(int) color.g};{(int) color.b}m";
	public override string fg() => $"\x1b[38;2;{(int) color.r};{(int) color.g};{(int) color.b}m";
}
