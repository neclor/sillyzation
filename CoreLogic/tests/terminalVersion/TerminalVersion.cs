global using TCell = CoreLogic.ICell<(uint x, uint y)>;
using System.Text;
using session;
using CoreLogic;
using AC = AnsiColors;

internal class TerminalVersion {
	private (int x, int y) map_size { get; }
	private Coord map_size_u { get; }
	private readonly SimpleMenu menu;
	private readonly TerminalMap map;
	private uint map_mode;
	private Coord? selected_cell;
	private Coord? previous_cell;

	public TerminalVersion(ISession<Coord> session, (int x, int y) map_size) {
		this.map_size = map_size;
		map_size_u = ((uint) this.map_size.x, (uint) this.map_size.y);

		Dictionary<PlayerKey, ISessionPlayer> players = session.getAllPlayers();

		TopBar topBar = new(() => ("Country", AC.STD_GOLD, "Test123"));
		Menu menuSelection = new(32);
		map = new(
			((uint) map_size.x, (uint) map_size.y),
			c => session.getCell(session.currentPlayerId, c),
			(cells) => {
				switch (map_mode) {
					case 0:
						return (cell) => TerminalMap.getTerrainTexture(cell.terrain);
					case 1:
						uint max_pop = 1;
						foreach (TCell cell in cells) {
							if (cell.population > max_pop) {
								max_pop = cell.population;
							}
						}
						return (cell) => {
							float pop_ratio = (float) cell.population / max_pop;
							uint red_value = (uint) (pop_ratio * 150);
							return new UniformCellTexture(new ColTrue(red_value, 0, 0));
						};
					case 2:
						return (cell) => new UniformCellTexture(new ColTrue(40, 40, 40));
					default:
						return (cell) => new UniformCellTexture(new ColTrue(0, 0, 0));
				}
			},
			(cell, neighbours) => {
				Highlights res = (null, null, null, null);
				if (cell.owner != null) {
					if (neighbours.top == null || neighbours.top.owner != cell.owner) {
						res.top = AC.getAnsiColor(players[cell.owner.Value].color);
					}
					if (neighbours.bot == null || neighbours.bot.owner != cell.owner) {
						res.bot = AC.getAnsiColor(players[cell.owner.Value].color);
					}
					if (neighbours.left == null || neighbours.left.owner != cell.owner) {
						res.left = AC.getAnsiColor(players[cell.owner.Value].color);
					}
					if (neighbours.right == null || neighbours.right.owner != cell.owner) {
						res.right = AC.getAnsiColor(players[cell.owner.Value].color);
					}
				}
				if (previous_cell != null && previous_cell.Value == cell.id) {
					res.top = res.right = res.left = res.bot = AC.STD_GRAY;
				}
				if (selected_cell != null && selected_cell.Value == cell.id) {
					res.top = res.right = res.left = res.bot = AC.STD_WHITE;
				}
				return res;
			},
			() => session.getAllUnitsVisibleFromPlayer(session.currentPlayerId).Value,
			(player) => AC.getAnsiColor(players[player].color)
		);

		Grid defaultMenu = new(
			() => AC.getAnsiColor(session.currentPlayer.color),
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
			selected_cell = null;
			previous_cell = null;
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
			Console.WriteLine(initial_coord);
			selected_cell = coord;
			previous_cell = initial_coord;
			Pixel[,] screen = defaultMenu.display();
			printScreen(screen);
		}

		menu = new("", "Choose your option :", true, [
			new ExecuteAndExitOption(" ⏎ End Turn", session.endTurn),
			new SimpleMenu(" ○ Change map mode", "Choose your map mode", true, [
				new GoBackOption(" ↩ Go Back"),
				new ExecuteAndContinueOption(" ○ Default map mode", () => map_mode = 0),
				new ExecuteAndContinueOption(" ○ Population map mode", () => map_mode = 1),
				new ExecuteAndContinueOption(" ○ Ressource map mode", () => map_mode = 2),
			], displayDefaultMenu),
			new DynamicMenu<MapUnit<Coord>>(" ○ Move Units", "Select Unit", false, [
					new GoBackOption(" ↩ Go Back"),
				],
				(unit) => new SelectCellMenu($" ○ {unit.name}", "Move unit to :", false, map_size_u, unit.position,
					c => new ExecuteAndContinueOption($"Unit {unit.name} to ({c.x}, {c.y})", () => session.moveUnit(session.currentPlayerId, unit.id, c)),
					displaySelectCellMenu
				),
				() => session.getAllUnitsOfPlayer(session.currentPlayerId),
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

		while (true) {
			_ = menu.execute();
		}
	}

	private static string loadingBar(uint prcnt) {
		const int len = 5;
		int i = ((int) prcnt) * len / 100;
		return new string('█', i) + new string('░', len - i);
	}

	private static void printScreen(Pixel[,] screen) {
		StringBuilder sb = new();
		Pixel? p = null;
		Pixel? prev;
		for (int y = 0; y < screen.GetLength(1); y++) {
			for (int x = 0; x < screen.GetLength(0); x++) {
				prev = p;
				p = screen[x, y] ?? new Pixel(' ');
				if (prev == null || prev.text_color != p.text_color) {
					_ = sb.Append(p.text_color.fg());
				}
				if (prev == null || prev.background_color != p.background_color) {
					_ = sb.Append(p.background_color.bg());
				}
				_ = sb.Append(p.c);
			}
			_ = sb.AppendLine("\x1b[0m");
			p = null;
		}
		Console.Write(new string('\n', Console.WindowHeight));
		Console.Write("\x1b[H");
		Console.WriteLine(sb.ToString());
	}
}