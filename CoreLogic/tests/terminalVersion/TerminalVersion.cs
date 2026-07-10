global using TUnit = CoreLogic.Unit<(uint x, uint y)>;
global using TCell = CoreLogic.ICell<(uint x, uint y)>;
global using Neighbours = (CoreLogic.ICell<(uint x, uint y)>? top, CoreLogic.ICell<(uint x, uint y)>? bot, CoreLogic.ICell<(uint x, uint y)>? left, CoreLogic.ICell<(uint x, uint y)>? right);
using System.Text;
using session;
using CoreLogic;
using AC = AnsiColors;
using ErrorOr;

internal class TerminalVersion {
	private ISession<Coord> session { get; }
	private (int x, int y) map_size { get; }
	private Coord map_size_u { get; }
	private readonly Dictionary<PlayerKey, ISessionPlayer> players;
	private readonly SimpleMenu menu;
	private readonly TerminalMap map;
	private uint map_mode;

	private static readonly (int x, int y) cell_size = (5, 4);
	private const int minMenuWidth = 32;

	public TerminalVersion(ISession<Coord> session, (int x, int y) map_size) {
		this.session = session;
		this.map_size = map_size;
		map_size_u = ((uint) this.map_size.x, (uint) this.map_size.y);

		players = session.getAllPlayers();

		menu = new("", "Choose your option :", true, [
			new ExecuteAndExitOption(" ⏎ End Turn", session.endTurn),
			new SimpleMenu(" ○ Change map mode", "Choose your map mode", false, [
				new GoBackOption(" ↩ Go Back"),
				new ExecuteAndContinueOption(" ○ Default map mode", () => map_mode = 0),
				new ExecuteAndContinueOption(" ○ Population map mode", () => map_mode = 1),
				new ExecuteAndContinueOption(" ○ Ressource map mode", () => map_mode = 2),
			], defaultMenu),
			new DynamicMenu<TUnit>(" ○ Move Units", "Select Unit", false, [
					new GoBackOption(" ↩ Go Back"),
				],
				(arg) => new SelectCellMenu($" ○ {arg}-0", "Move unit to :", false, map_size_u, (2, 2),
					c => new ExecuteAndContinueOption($"Unit {arg} to ({c.x}, {c.y})", () => {}),
					printSelectCellMenu
				),
				() => session.getAllUnits(session.currentPlayerId),
				defaultMenu
			),
			new DynamicMenu<QueueKey>(" ○ Unit Queue", "Select Unit Queue", true,
				[
					new GoBackOption(" ↩ Go Back"),
					new ExecuteAndContinueOption(" + New Unit Queue", () => session.createUnitQueueGroup(session.currentPlayerId)),
				],
				queue => new DynamicMenu<TUnit>($" ○ {queue}", $"Unit Queue : {queue}", true,
					[
						new GoBackOption(" ↩ Go Back"),
						new SimpleMenu(" + Add new unit to unit Queue", "Select new unit type", false, [
							new GoBackOption(" ↩ Go Back"),
							new ExecuteAndContinueOption(" ○ Infantry", () => session.addUnitToQueue(session.currentPlayerId, queue, new Infantry<Coord>(session.currentPlayerId))),
							new ExecuteAndContinueOption(" ○ Tank", () => session.addUnitToQueue(session.currentPlayerId, queue, new Tank<Coord>(session.currentPlayerId))),
						], defaultMenu),
					],
					(unit) => new SimpleMenu($" ○ Unit {unit.name}", $"Actions for Unit {unit.name}", false, [
						new GoBackOption(" ↩ Go Back"),
						new ConditionalOption(
							new SelectCellMenu(" ○ Deploy", "Choose where to deploy", false, map_size_u, null,
								(pos) => new ExecuteAndContinueOption(" ○ Deploy", () => session.deployUnitFromQueue(session.currentPlayerId, queue, unit.id, pos)),
								printSelectCellMenu
							),
							() => unit.id == 1
						),
						new ExecuteAndContinueOption(" ○ Delete", () => session.deleteUnitFromQueue(session.currentPlayerId, queue, unit.id))
					], defaultMenu),
					() => session.getAllUnitInQueue(session.currentPlayerId, queue),
					defaultMenu
				),
				() => session.getAllUnitQueueId(session.currentPlayerId),
				defaultMenu
			),
		], defaultMenu);

		map = new(
			((uint) map_size.x, (uint) map_size.y),
			playerId => players[playerId].color,
			c => session.getCell(session.currentPlayerId, c)
		);
	}

	private void defaultMenu(string name, (string option, bool is_highlighted)[] options) {
		print(
			[
				(name, ""),
				..options.Select((option, index) => (option.option, option.is_highlighted ? AC.BG_STD_GOLD : ""))
			]
		);
	}

	private void printSelectCellMenu(string title, Coord? initial_coord, Coord coord) {
		if (initial_coord.HasValue) {
			print([
				(title, "")
			], [
				(coord, AC.BG_STD_WHITE, 1, null),
				(initial_coord.Value, AC.BG_STD_GRAY, 0, null)
			]);
		}
		else {
			print([
				(title, "")
			], [
				(coord, AC.BG_STD_WHITE, 1, null),
			]);
		}
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

	private void print(
		(string content,
		string color)[] contentMenu,
		(Coord coord, string color, uint priority, Func<TCell, TCell?, bool>? highligh)[]? highlighted_coords = null
	) {
		int longest = (contentMenu.Length != 0)
			? contentMenu.Max((cur) => cur.content.Length)
			: 0;
		int menuWidth = Math.Max(longest, minMenuWidth);
		int mapWidth = map_size.x * cell_size.x * TerminalMap.cell_width_ration;
		int mapHeight = map_size.y * cell_size.y;
		int nbLines = contentMenu.Length > mapHeight ? contentMenu.Length : mapHeight;

		List<string> map_res = map_mode switch {
			0 => map.printDefaultMap(highlighted_coords),
			1 => map.printPopMap(highlighted_coords),
			2 => map.printRessourceMap(highlighted_coords),
			_ => throw new InvalidDataException("Invalid map mode index"),
		};

		string textColor = getAnsiTextColor(session.currentPlayer.color);
		StringBuilder sb = new();
#pragma warning disable IDE0058 // Expression value is never used

		sb.AppendLine($"{textColor}╔{new('═', menuWidth + mapWidth)}═══╗{AC.RESET}");
		sb.AppendLine($"{textColor}║{AC.RESET}{$" Country: {textColor}{session.currentPlayer.name}{AC.RESET}   Population: 10000   Iron: 6769{new(' ', menuWidth + 1 + mapWidth - session.currentPlayer.name.Length - 41)}"}{textColor}║{AC.RESET}");
		sb.AppendLine($"{textColor}╠{new('═', menuWidth)}╦{new('═', mapWidth)}══╣{AC.RESET}");
		foreach (((string content, string color), int i) in contentMenu.Select((value, index) => (value, index))) {
			sb.Append($"{textColor}║{AC.RESET}{color}{content.PadRight(menuWidth)}{AC.RESET}{textColor}║{AC.RESET} ");
			if (i >= mapHeight) {
				sb.Append(' ', mapWidth);
			}
			else {
				sb.Append(map_res[i]);
			}
			sb.AppendLine($" {textColor}║{AC.RESET}");
		}
		if (contentMenu.Length < mapHeight) {
			string leftPad = $"{textColor}║{AC.RESET}{new(' ', menuWidth)}{textColor}║{AC.RESET}";
			string rightPad = $"{textColor}║{AC.RESET}";

			for (int i = contentMenu.Length; i < mapHeight; i++) {
				sb.AppendLine($"{leftPad} {map_res[i]} {rightPad}");
			}
		}
		sb.AppendLine($"{textColor}╚{new('═', menuWidth)}╩{new('═', mapWidth)}══╝{AC.RESET}");
#pragma warning restore IDE0058 // Expression value is never used

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
			_ = menu.execute();
		}
	}
}