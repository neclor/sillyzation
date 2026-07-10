internal interface ITerminalMenuOption {
	string? name { get; }
	MenuResult execute();
}

internal class GoBackOption : ITerminalMenuOption {
	public string name { get; }

	public GoBackOption(string name) => this.name = name;

	public MenuResult execute() {
		return MenuResult.GoBack;
	}
}

internal class ExecuteAndContinueOption : ITerminalMenuOption {
	public string name { get; }
	private readonly Action func;

	public ExecuteAndContinueOption(
		string name,
		Action func
	) {
		this.name = name;
		this.func = func;
	}

	public MenuResult execute() {
		func();
		return MenuResult.GoBackToRoot;
	}
}

internal class ExecuteAndExitOption : ITerminalMenuOption {
	public string name { get; }
	private readonly Action func;

	public ExecuteAndExitOption(
		string name,
		Action func
	) {
		this.name = name;
		this.func = func;
	}

	public MenuResult execute() {
		func();
		return MenuResult.ExitAll;
	}
}

internal class ConditionalOption : ITerminalMenuOption {
	private readonly ITerminalMenuOption option;
	private readonly Func<bool> can_show;

	public ConditionalOption(ITerminalMenuOption option, Func<bool> can_show) {
		this.option = option;
		this.can_show = can_show;
	}

	public string? name => can_show() ? option.name : null;

	public MenuResult execute() {
		return option.execute();
	}
}