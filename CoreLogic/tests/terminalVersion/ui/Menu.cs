using AC = AnsiColors;

internal class Menu : IUserInterfaceTerminal {
	private static readonly AC highlight_color = AC.STD_GOLD;
	private (string content, bool highlighted)[] contents = [];
	private readonly int min_width;

	public Menu(int min_width) => this.min_width = min_width;

	public void setContent((string content, bool highlighted)[] contents) {
		this.contents = contents;
	}

	public Pixel[,] display() {
		int longest = contents.Max((c) => c.content.Length);
		if (longest < min_width) {
			longest = min_width;
		}

		Pixel[,] res = new Pixel[longest, contents.Length];

		foreach ((int index, (string content, bool highlighted)) in contents.Index()) {
			AC color = highlighted ? highlight_color : AC.RESET;
			foreach ((int i, char c) in content.Index()) {
				res[i, index] = new Pixel(c, color);
			}
			for (int i = content.Length; i < longest; i++) {
				res[i, index] = new Pixel(color);
			}
		}
		return res;
	}
}
