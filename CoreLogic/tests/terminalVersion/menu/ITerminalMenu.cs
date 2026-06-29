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