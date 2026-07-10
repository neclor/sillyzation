using ErrorOr;

internal enum MenuResult {
	Continue,
	GoBack,
	ExitAll,
	GoBackToRoot
}

internal abstract class BaseMenu : ITerminalMenuOption {
	public string name { get; }
	protected readonly string title;
	private readonly bool is_root;

	protected abstract MenuResult handleKey(ConsoleKey input);
	protected abstract void displayMenu();

	protected BaseMenu(
		string name,
		string title,
		bool is_root
	) {
		this.name = name;
		this.title = title;
		this.is_root = is_root;
	}

	public MenuResult execute() {
		while (true) {
			clear();
			displayMenu();
			ConsoleKey input = Console.ReadKey(true).Key;
			MenuResult status = handleKey(input);
			switch (status) {
				case MenuResult.GoBack:
					return MenuResult.Continue;
				case MenuResult.GoBackToRoot:
					if (!is_root) {
						return MenuResult.GoBackToRoot;
					}
					break;
				case MenuResult.ExitAll:
					return MenuResult.ExitAll;
				case MenuResult.Continue:
					break;
				default:
					break;
			}
		}
	}

	private static void clear() {
		Console.Write(new string('\n', Console.WindowHeight));
		Console.Write("\x1b[H");
	}
}





internal class SimpleMenu : BaseMenu {
	private readonly ITerminalMenuOption[] options;
	private uint option_index;
	private readonly Action<string, (string option, bool is_highlighted)[]> display_func;

	public SimpleMenu(
		string name,
		string title,
		bool is_root,
		ITerminalMenuOption[] options,
		Action<string, (string option, bool is_highlighted)[]> display_func
	) : base(name, title, is_root) {
		option_index = 0;
		this.options = options;
		this.display_func = display_func ?? throw new ArgumentNullException(nameof(display_func));
	}

	protected override void displayMenu() {
		(string option, bool is_highlighted)[] option_names = [..
			options
				.Select((o, i) => (o.name, is_highlighted: i == option_index))
				.Where(o => o.name != null)
				.Select(o => (option: o.name!, o.is_highlighted))
		];

		display_func(title, option_names);
	}

	protected override MenuResult handleKey(ConsoleKey input) {
#pragma warning disable IDE0010 // Add missing cases
		switch (input) {
			case ConsoleKey.UpArrow:
				if (option_index > 0) {
					option_index--;
				}
				while (option_index > 0 && options[option_index].name == null) {
					option_index--;
				}
				return MenuResult.Continue;
			case ConsoleKey.DownArrow:
				if (option_index < options.Length - 1) {
					option_index++;
				}
				while (option_index < options.Length - 1 && options[option_index].name == null) {
					option_index++;
				}
				return MenuResult.Continue;
			case ConsoleKey.Enter:
				MenuResult result = options[option_index].execute();
				option_index = 0;
				return result;
			default:
				return MenuResult.Continue;
		}
#pragma warning restore IDE0010 // Add missing cases
	}
}





internal class DynamicMenu<T> : BaseMenu {
	private int option_index;
	private ITerminalMenuOption[] options;
	private readonly ITerminalMenuOption[] static_options;
	private readonly Func<T, ITerminalMenuOption> factory;
	private readonly Func<ErrorOr<T[]>> get_values;
	private readonly Action<string, (string option, bool is_highlighted)[]> display_func;

	public DynamicMenu(
		string name,
		string title,
		bool is_root,
		ITerminalMenuOption[] static_options,
		Func<T, ITerminalMenuOption> factory,
		Func<ErrorOr<T[]>> get_values,
		Action<string, (string option, bool is_highlighted)[]> display_func
	) : base(name, title, is_root) {
		option_index = 0;
		options = [];
		this.static_options = static_options;
		this.factory = factory;
		this.get_values = get_values;
		this.display_func = display_func;
	}

	protected override void displayMenu() {
		ErrorOr<T[]> fetch_values = get_values();
		T[] values = fetch_values.IsError ? [] : (fetch_values.Value ?? []);
		IEnumerable<ITerminalMenuOption> dynamic_options = values.Select(e => factory(e));
		options = [.. static_options, .. dynamic_options];

		(string option, bool is_highlighted)[] option_names = [..
			options
				.Select((o, i) => (o.name, is_highlighted: i == option_index))
				.Where(o => o.name != null)
				.Select(o => (option: o.name!, o.is_highlighted))
		];

		display_func(
			title,
			option_names
		);
	}

	protected override MenuResult handleKey(ConsoleKey input) {
#pragma warning disable IDE0010 // Add missing cases
		switch (input) {
			case ConsoleKey.UpArrow:
				if (option_index > 0) {
					option_index--;
				}
				while (option_index > 0 && options[option_index].name == null) {
					option_index--;
				}
				return MenuResult.Continue;
			case ConsoleKey.DownArrow:
				if (option_index < options.Length - 1) {
					option_index++;
				}
				while (option_index < options.Length - 1 && options[option_index].name == null) {
					option_index++;
				}
				return MenuResult.Continue;
			case ConsoleKey.Enter:
				ITerminalMenuOption option = options[option_index];
				option_index = 0;
				return option.execute();
			default:
				return MenuResult.Continue;
		}
#pragma warning restore IDE0010 // Add missing cases
	}
}





internal class SelectCellMenu : BaseMenu {
	private Coord coord;
	private readonly Coord map_size;
	private readonly Coord? initial_coord;

	private readonly Func<Coord, ITerminalMenuOption> factory;
	private readonly Action<string, Coord?, Coord> display_func;

	public SelectCellMenu(
		string name,
		string title,
		bool is_root,
		Coord map_size,
		Coord? initial_coord,
		Func<Coord, ITerminalMenuOption> factory,
		Action<string, Coord?, Coord> display_func
	) : base(name, title, is_root) {
		this.map_size = map_size;
		this.initial_coord = initial_coord;
		coord = this.initial_coord ?? (1, 1);
		this.factory = factory;
		this.display_func = display_func;
	}

	protected override void displayMenu() {
		display_func(title, initial_coord, coord);
	}

	protected override MenuResult handleKey(ConsoleKey input) {
#pragma warning disable IDE0010 // Add missing cases
		switch (input) {
			case ConsoleKey.UpArrow:
				if (coord.y > 1) {
					coord.y--;
				}
				return MenuResult.Continue;
			case ConsoleKey.DownArrow:
				if (coord.y < map_size.y) {
					coord.y++;
				}
				return MenuResult.Continue;
			case ConsoleKey.LeftArrow:
				if (coord.x > 1) {
					coord.x--;
				}
				return MenuResult.Continue;
			case ConsoleKey.RightArrow:
				if (coord.x < map_size.x) {
					coord.x++;
				}
				return MenuResult.Continue;
			case ConsoleKey.Enter:

				ITerminalMenuOption option = factory(coord);
				coord = initial_coord ?? (1, 1);
				return option.execute();
			default:
				return MenuResult.Continue;
		}
#pragma warning restore IDE0010 // Add missing cases
	}
}
