global using TCell = CoreLogic.ICell<(uint x, uint y)>;
using System.Text;
using session;
using CoreLogic;
using AC = AnsiColors;

internal class TerminalVersion {
	private ISession<Coord> session { get; }
	private (int x, int y) map_size { get; }
	private Coord map_size_u { get; }
	private readonly SimpleMenu menu;
	private readonly TerminalMap map;
	private uint map_mode;

	public TerminalVersion(ISession<Coord> session, (int x, int y) map_size) {
		this.session = session;
		this.map_size = map_size;
		map_size_u = ((uint) this.map_size.x, (uint) this.map_size.y);

		TopBar topBar = new(() => ("Country", AC.STD_GOLD, "Test123"));
		Menu menuSelection = new();
		map = new(
			((uint) map_size.x, (uint) map_size.y),
			c => session.getCell(session.currentPlayerId, c),
			(cell) => {
				return TerminalMap.getTerrainTexture(cell.terrain);
			},
			(c, neighbours) => {
				return (null, null, null, null);
			}
		);

		Grid defaultMenu = new(
			new int[,] {
				{ 0, 0 },
				{ 1, 2 },
			},
			[
				topBar,
				menuSelection,
				map
			]
		);

		void displayDefaultMenu(
			string title,
			(string option, bool is_highlighted)[] options
		) {
			menuSelection.setContent([
				(title, false),
				.. options
			]);
			Pixel[,] screen = defaultMenu.display();
			printScreen(screen);
		}

		void displaySelectCellMenu(
			string title,
			Coord? initial_coord,
			Coord coord
		) {
			menuSelection.setContent([
				(title, false),
			]);
			Pixel[,] screen = defaultMenu.display();
			printScreen(screen);
		}

		menu = new("", "Choose your option :", true, [
			new ExecuteAndExitOption(" ⏎ End Turn", session.endTurn),
			new SimpleMenu(" ○ Change map mode", "Choose your map mode", false, [
				new GoBackOption(" ↩ Go Back"),
				new ExecuteAndContinueOption(" ○ Default map mode", () => map_mode = 0),
				new ExecuteAndContinueOption(" ○ Population map mode", () => map_mode = 1),
				new ExecuteAndContinueOption(" ○ Ressource map mode", () => map_mode = 2),
			], displayDefaultMenu),
			new DynamicMenu<MapUnit<Coord>>(" ○ Move Units", "Select Unit", false, [
					new GoBackOption(" ↩ Go Back"),
				],
				(arg) => new SelectCellMenu($" ○ {arg}-0", "Move unit to :", false, map_size_u, (2, 2),
					c => new ExecuteAndContinueOption($"Unit {arg} to ({c.x}, {c.y})", () => {}),
					displaySelectCellMenu
				),
				() => session.getAllUnits(session.currentPlayerId),
				displayDefaultMenu
			),
			new DynamicMenu<QueueKey>(" ○ Unit Queue", "Select Unit Queue", true,
				[
					new GoBackOption(" ↩ Go Back"),
					new ExecuteAndContinueOption(" + New Unit Queue", () => session.createUnitQueueGroup(session.currentPlayerId)),
				],
				queue => new DynamicMenu<QueueUnit<Coord>>($" ○ {queue}", $"Unit Queue : {queue}", true,
					[
						new GoBackOption(" ↩ Go Back"),
						new SimpleMenu(" + Add new unit to unit Queue", "Select new unit type", false, [
							new GoBackOption(" ↩ Go Back"),
							new ExecuteAndContinueOption(" ○ Infantry", () => session.addUnitToQueue(session.currentPlayerId, queue, new Infantry<Coord>(session.currentPlayerId).toQueue())),
							new ExecuteAndContinueOption(" ○ Tank", () => session.addUnitToQueue(session.currentPlayerId, queue, new Tank<Coord>(session.currentPlayerId).toQueue())),
							new ExecuteAndContinueOption(" ○ Artillery", () => session.addUnitToQueue(session.currentPlayerId, queue, new Artillery<Coord>(session.currentPlayerId).toQueue())),
						], displayDefaultMenu),
					],
					(unit) => new SimpleMenu($" [{loadingBar(unit.progress)}] Unit {unit.name}", $"Actions for [{loadingBar(unit.progress)}] {unit.name} ", false, [
						new GoBackOption(" ↩ Go Back"),
						new ConditionalOption(
							new SelectCellMenu(" ○ Deploy", "Choose where to deploy", false, map_size_u, null,
								(pos) => new ExecuteAndContinueOption(" ○ Deploy", () => session.deployUnitFromQueue(session.currentPlayerId, queue, unit.id, pos)),
								displaySelectCellMenu
							),
							() => unit.ready
						),
						new ExecuteAndContinueOption(" ○ Delete", () => session.deleteUnitFromQueue(session.currentPlayerId, queue, unit.id))
					], displayDefaultMenu),
					() => session.getAllUnitInQueue(session.currentPlayerId, queue),
					displayDefaultMenu
				),
				() => session.getAllUnitQueueId(session.currentPlayerId),
				displayDefaultMenu
			),
		], displayDefaultMenu);
	}

	private static string loadingBar(uint prcnt) {
		const int len = 5;
		int i = ((int) prcnt) * len / 100;
		return new string('█', i) + new string('░', len - i);
	}

	private static void printScreen(Pixel[,] screen) {
		StringBuilder sb = new();
		for (var y = 0; y < screen.GetLength(1); y++) {
			for (var x = 0; x < screen.GetLength(0); x++) {
				Pixel p = screen[x, y] ?? new Pixel(' ');
				_ = sb.Append(p.background_color.bg())
					.Append(p.background_color.fg())
					.Append(p.c);
			}
			_ = sb.AppendLine();
		}
		Console.Write(new string('\n', Console.WindowHeight));
		Console.Write("\x1b[H");
		Console.WriteLine(sb.ToString());
	}

	public void start() {
		while (true) {
			_ = menu.execute();
		}
	}
}