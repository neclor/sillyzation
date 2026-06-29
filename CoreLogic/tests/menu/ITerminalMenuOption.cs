internal interface ITerminalMenuOption {
	string name { get; }
	MenuResult execute();
}

internal class GoBackOption : ITerminalMenuOption {
	public string name { get; }

	public GoBackOption(string name) => this.name = name;

	public MenuResult execute() {
		return MenuResult.GoBack;
	}
}

internal class ExecuteOption : ITerminalMenuOption {
	public string name { get; }
	private readonly Func<MenuResult?> func;

	public ExecuteOption(
		string name,
		Func<MenuResult?> func
	) {
		this.name = name;
		this.func = func;
	}

	public MenuResult execute() {
		return func() ?? MenuResult.GoBackToRoot;
	}
}