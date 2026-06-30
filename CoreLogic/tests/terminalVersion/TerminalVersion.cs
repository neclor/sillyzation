using System.Text;
using session;
using CoreLogic;
using System.Globalization;
using AC = AnsiColors;

internal class TerminalVersion {
	private ISession<(uint, uint)> session { get; }
	private (int x, int y) map_size { get; }
	private Dictionary<uint, IPlayer> players;
	private readonly SimpleMenu menu;

	private static readonly (int x, int y) cell_size = (5, 4);
	private const int minMenuWidth = 32;

	public TerminalVersion(ISession<(uint, uint)> session, (int, int) map_size) {
		this.session = session;
		this.map_size = map_size;
		players = session.getAllPlayers();
		menu = new("Choose your option :", "", true, [
			new ExecuteAndExitOption("End Turn", session.endTurn),
			new DynamicMenu<string>(
				"Select Unit",
				"Move Units",
				[
					new GoBackOption("Go Back"),
				],
				(arg) => new DynamicMenu<(uint, uint)>(
					$"From {arg} to:", $"From {arg}", [
						new GoBackOption("Go Back"),
					],
					((uint x, uint y) c) =>
						new ExecuteAndContinueOption($"Unit {arg} to ({c.x}, {c.y})", () => {}
					),
					() => [(1,1),(1,2),(1,3),(1,4)],
					defaultMenu
				),
				() => ["Hello", "World", "Stupid"],
				defaultMenu
			),
			new SimpleMenu("Unit Queue actions:", "Unit Queue", false, [
				new GoBackOption("Go Back"),
				new ExecuteAndContinueOption("New Unit Queue", () => {}),
				new ExecuteAndContinueOption("Add new unit to unit Queue", () => {}),
				new ExecuteAndContinueOption("Deploy Unit Queue", () => {}),
			], defaultMenu),
		], defaultMenu);
	}

	private void defaultMenu(string name, (string option, int index)[] options, int selected) {
		print(
			[
				(name, ""),
				..options.Select((option, index) => (option.option, option.index == selected ? AC.BG_STD_GOLD : ""))
			]
		);
	}

	public static string getAnsiTextColor(Color color) => color switch {
		Color.Red => AC.FG_STD_RED,
		Color.Gold => AC.FG_STD_GOLD,
		Color.Orange => AC.FG_STD_ORANGE,
		Color.Yellow => AC.FG_STD_YELLOW,
		Color.LightGreen => AC.FG_STD_LIGHT_GREEN,
		Color.DarkGreen => AC.FG_STD_DARK_GREEN,
		Color.Green => AC.FG_STD_GREEN,
		Color.LightBlue => AC.FG_STD_CYAN,
		Color.Blue => AC.FG_STD_BLUE,
		Color.Purple => AC.FG_STD_PURPLE,
		Color.White => AC.FG_STD_WHITE,
		Color.Gray => AC.FG_STD_GRAY,
		Color.Brown => AC.FG_STD_BROWN,
		_ => AC.RESET
	};

	private void print((string content, string color)[] contentMenu) {
		int longest = (contentMenu.Length != 0)
			? contentMenu.Max((cur) => cur.content.Length)
			: 0;
		int menuWidth = Math.Max(longest, minMenuWidth);
		int mapWidth = map_size.x * cell_size.x * 2;
		int mapHeight = map_size.y * cell_size.y;
		int nbLines = contentMenu.Length > mapHeight ? contentMenu.Length : mapHeight;

		List<string> map = TerminalMap.printMap(
			map_size,
			playerId => players[playerId].color,
			c => session.getCell(session.currentPlayerId, c)
		);

		string textColor = getAnsiTextColor(session.currentPlayer.color);
		StringBuilder sb = new();
		_ = sb.AppendLine(
			CultureInfo.InvariantCulture,
			$"{textColor}╔{new string('═', menuWidth)}╦{new string('═', mapWidth)}╗{AC.RESET}"
		);
		foreach (((string content, string color), int i) in contentMenu.Select((value, index) => (value, index))) {
			_ = sb.Append(CultureInfo.InvariantCulture, $"{textColor}║{AC.RESET}{color}{content.PadRight(menuWidth)}{AC.RESET}{textColor}║{AC.RESET}");
			if (i >= mapHeight) {
				_ = sb.Append(' ', mapWidth);
			}
			else {
				_ = sb.Append(map[i]);
			}
			_ = sb.AppendLine(CultureInfo.InvariantCulture, $"{textColor}║{AC.RESET}");
		}
		if (contentMenu.Length < mapHeight) {
			string leftPad = $"{textColor}║{AC.RESET}{new string(' ', menuWidth)}{textColor}║{AC.RESET}";
			string rightPad = $"{textColor}║{AC.RESET}";

			for (int i = contentMenu.Length; i < mapHeight; i++) {
				_ = sb.Append(leftPad)
					.Append(map[i])
					.AppendLine(rightPad);
			}
		}
		_ = sb.AppendLine(
			CultureInfo.InvariantCulture,
			$"{textColor}╚{new string('═', menuWidth)}╩{new string('═', mapWidth)}╝{AC.RESET}"
		);

		string res = sb.ToString();
		clear();
		Console.WriteLine(res);
	}

	private static void clear() {
		Console.Write(new string('\n', Console.WindowHeight));
		Console.Write("\x1b[H");
	}

	public void start() {
		while (true) {
			IPlayer player = session.currentPlayer;
			Console.WriteLine("Player : " + player.name);
			_ = menu.display();
		}
	}
}