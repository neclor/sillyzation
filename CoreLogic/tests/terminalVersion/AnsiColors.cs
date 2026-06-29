internal class AnsiColors {
	public const string RESET = "\x1b[0m";

	// High-Intensity Solid Entity Backgrounds (Guaranteed to pop)
	public const string BG_STD_RED = "\x1b[48;5;196m";
	public const string BG_STD_GOLD = "\x1b[48;5;220m";
	public const string BG_STD_ORANGE = "\x1b[48;5;208m";
	public const string BG_STD_YELLOW = "\x1b[48;5;226m";
	public const string BG_STD_LIGHT_GREEN = "\x1b[48;5;119m";
	public const string BG_STD_DARK_GREEN = "\x1b[48;5;40m";
	public const string BG_STD_GREEN = "\x1b[48;5;46m";
	public const string BG_STD_CYAN = "\x1b[48;5;87m";
	public const string BG_STD_BLUE = "\x1b[48;5;27m";
	public const string BG_STD_PURPLE = "\x1b[48;5;201m";
	public const string BG_STD_WHITE = "\x1b[48;5;255m";
	public const string BG_STD_GRAY = "\x1b[48;5;250m";
	public const string BG_STD_BROWN = "\x1b[48;5;172m";

	// --- Terrain Background Truecolor Variants ---
	// Plain
	public const string BG_PLAIN_1 = "\x1b[48;2;35;48;35m";
	public const string BG_PLAIN_2 = "\x1b[48;2;30;42;30m";
	public const string BG_PLAIN_3 = "\x1b[48;2;40;54;40m";
	public const string BG_PLAIN_4 = "\x1b[48;2;25;36;25m";

	// Forest
	public const string BG_FOREST_1 = "\x1b[48;2;15;45;18m";
	public const string BG_FOREST_2 = "\x1b[48;2;12;38;15m";
	public const string BG_FOREST_3 = "\x1b[48;2;18;52;21m";
	public const string BG_FOREST_4 = "\x1b[48;2;10;32;12m";

	// Desert
	public const string BG_DESERT_1 = "\x1b[48;2;145;85;35m";
	public const string BG_DESERT_2 = "\x1b[48;2;135;78;30m";
	public const string BG_DESERT_3 = "\x1b[48;2;155;92;40m";
	public const string BG_DESERT_4 = "\x1b[48;2;125;72;26m";

	// Tundra
	public const string BG_TUNDRA_1 = "\x1b[48;2;34;38;44m";
	public const string BG_TUNDRA_2 = "\x1b[48;2;30;34;40m";
	public const string BG_TUNDRA_3 = "\x1b[48;2;38;42;48m";
	public const string BG_TUNDRA_4 = "\x1b[48;2;26;29;34m";

	// Savanna
	public const string BG_SAVANNA_1 = "\x1b[48;2;115;85;25m";
	public const string BG_SAVANNA_2 = "\x1b[48;2;105;77;20m";
	public const string BG_SAVANNA_3 = "\x1b[48;2;125;93;30m";
	public const string BG_SAVANNA_4 = "\x1b[48;2;95;69;16m";

	// Swamp
	public const string BG_SWAMP_1 = "\x1b[48;2;18;40;38m";
	public const string BG_SWAMP_2 = "\x1b[48;2;15;34;32m";
	public const string BG_SWAMP_3 = "\x1b[48;2;22;46;44m";
	public const string BG_SWAMP_4 = "\x1b[48;2;12;28;26m";

	// Jungle
	public const string BG_JUNGLE_1 = "\x1b[48;2;32;72;36m";
	public const string BG_JUNGLE_2 = "\x1b[48;2;24;54;30m";
	public const string BG_JUNGLE_3 = "\x1b[48;2;40;90;42m";
	public const string BG_JUNGLE_4 = "\x1b[48;2;16;36;24m";
}