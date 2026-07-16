using AC = AnsiColors;

internal class TopBar : IUserInterfaceTerminal {
	private readonly Func<(string country, AC country_color, string info)> get_current_info;

	public TopBar(Func<(string country, AC country_color, string info)> get_current_info) {
		this.get_current_info = get_current_info;
	}

	public Pixel[,] display() {
		(string country, AC country_color, string info) = get_current_info();

		string to_print = $"Country: {country} {info}";

		Pixel[,] result = new Pixel[to_print.Length, 1];
		foreach ((int i, char c) in to_print.Index()) {
			result[i, 0] = new Pixel(c);
		}

		return result;
	}
}
