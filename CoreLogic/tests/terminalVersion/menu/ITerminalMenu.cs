internal enum MenuResult {
	Continue,
	GoBack,
	ExitAll,
	GoBackToRoot
}

internal interface ITerminalMenu {
	MenuResult display();
}

internal class SimpleMenu : ITerminalMenu, ITerminalMenuOption {
	public readonly string name;
	private readonly string option_name;
	private readonly ITerminalMenuOption[] options;
	private readonly bool is_root;
	private readonly Action<string, (string option, int index)[], int> display_func;
	private int option_index;

	string ITerminalMenuOption.name => option_name;

	public SimpleMenu(
		string name,
		string option_name,
		bool is_root,
		ITerminalMenuOption[] options,
		Action<string, (string option, int index)[], int> display_func
	) {
		this.name = name;
		this.option_name = option_name;
		this.options = options;
		this.is_root = is_root;
		this.display_func = display_func;
		option_index = 0;
	}

	private static void clear() {
		Console.Write(new string('\n', Console.WindowHeight));
		Console.Write("\x1b[H");
	}

	public MenuResult display() {
		while (true) {
			clear();
			display_func(name, [.. options.Select((value, index) => (value.name, index))], option_index);
			ConsoleKey input = Console.ReadKey().Key;
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

	public MenuResult execute() {
		return display();
	}

	private MenuResult handleKey(ConsoleKey input) {
#pragma warning disable IDE0010 // Add missing cases
		switch (input) {
			case ConsoleKey.UpArrow:
				option_index = Math.Max(option_index - 1, 0);
				Console.WriteLine($"index {option_index}");
				return MenuResult.Continue;
			case ConsoleKey.DownArrow:
				option_index = Math.Min(option_index + 1, options.Length - 1);
				Console.WriteLine($"index {option_index}");
				return MenuResult.Continue;
			case ConsoleKey.Enter:
				ITerminalMenuOption option = options[option_index];
				return option.execute();
			default:
				return MenuResult.Continue;
		}
#pragma warning restore IDE0010 // Add missing cases
	}
}

internal class DynamicMenu<T> : ITerminalMenu, ITerminalMenuOption {
	private readonly string name;
	private readonly string option_name;
	private readonly ITerminalMenuOption[] static_options;
	private readonly Func<T, ITerminalMenuOption> factory;
	private readonly Func<T[]> get_values;
	private readonly Action<string, (string option, int index)[], int> display_func;

	public DynamicMenu(
		string name,
		string option_name,
		ITerminalMenuOption[] static_options,
		Func<T, ITerminalMenuOption> factory,
		Func<T[]> get_values,
		Action<string, (string option, int index)[], int> display_func
	) {
		this.name = name;
		this.option_name = option_name;
		this.static_options = static_options;
		this.factory = factory;
		this.get_values = get_values;
		this.display_func = display_func;
	}

	string ITerminalMenuOption.name => option_name;

	public MenuResult display() {
		T[] values = get_values();
		IEnumerable<ITerminalMenuOption> dynamic_options = values.Select(e => factory(e));
		return new SimpleMenu(
			name, "", false, [.. static_options, .. dynamic_options], display_func
		).display();
	}

	public MenuResult execute() {
		return display();
	}
}

internal class SelectCellMenu : ITerminalMenu, ITerminalMenuOption {
	private readonly string name;
	private readonly string option_name;
	private Coord coord;
	private readonly Coord map_size;
	private readonly Coord initial_coord;
	private readonly Func<Coord, ITerminalMenuOption> factory;
	private readonly Action<string, Coord, Coord> display_func;

	string ITerminalMenuOption.name => option_name;

	public SelectCellMenu(
		string option_name,
		string name,
		Coord map_size,
		Coord initial_coord,
		Func<Coord, ITerminalMenuOption> factory,
		Action<string, Coord, Coord> display_func
	) {
		this.name = name;
		this.option_name = option_name;
		this.map_size = map_size;
		this.initial_coord = initial_coord;
		this.coord = initial_coord;
		this.factory = factory;
		this.display_func = display_func;
	}

	private static void clear() {
		Console.Write(new string('\n', Console.WindowHeight));
		Console.Write("\x1b[H");
	}

	public MenuResult display() {
		while (true) {
			clear();
			display_func(name, initial_coord, coord);
			ConsoleKey input = Console.ReadKey().Key;
			MenuResult status = handleKey(input);
			switch (status) {
				case MenuResult.GoBack:
					return MenuResult.Continue;
				case MenuResult.GoBackToRoot:
					return MenuResult.GoBackToRoot;
				case MenuResult.ExitAll:
					return MenuResult.ExitAll;
				case MenuResult.Continue:
					break;
				default:
					break;
			}
		}
	}

	private MenuResult handleKey(ConsoleKey input) {
#pragma warning disable IDE0010 // Add missing cases
		switch (input) {
			case ConsoleKey.UpArrow:
				if (coord.y > 1) {
					coord.y--;
				}
				Console.WriteLine($"selected coord {coord}");
				return MenuResult.Continue;
			case ConsoleKey.DownArrow:
				if (coord.y < map_size.y) {
					coord.y++;
				}
				Console.WriteLine($"selected coord {coord}");
				return MenuResult.Continue;
			case ConsoleKey.LeftArrow:
				if (coord.x > 1) {
					coord.x--;
				}
				Console.WriteLine($"selected coord {coord}");
				return MenuResult.Continue;
			case ConsoleKey.RightArrow:
				if (coord.x < map_size.x) {
					coord.x++;
				}
				Console.WriteLine($"selected coord {coord}");
				return MenuResult.Continue;
			case ConsoleKey.Enter:
				ITerminalMenuOption option = factory(coord);
				return option.execute();
			default:
				return MenuResult.Continue;
		}
#pragma warning restore IDE0010 // Add missing cases
	}

	public MenuResult execute() {
		return display();
	}
}
