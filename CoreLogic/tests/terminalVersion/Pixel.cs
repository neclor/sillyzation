using CoreLogic;

internal class Pixel {
	public char c { get; set; }
	public AnsiColors text_color { get; set; }
	public AnsiColors background_color { get; set; }

	public Pixel(
		char c,
		AnsiColors text_color,
		AnsiColors background_color
	) {
		this.c = c;
		this.text_color = text_color;
		this.background_color = background_color;
	}

	public Pixel(
		char c,
		AnsiColors background_color
	) {
		this.c = c;
		this.background_color = background_color;
		text_color = AnsiColors.RESET;
	}

	public Pixel(
		char c
	) {
		this.c = c;
		background_color = AnsiColors.RESET;
		text_color = AnsiColors.RESET;
	}

	public Pixel(
		AnsiColors background_color
	) {
		c = ' ';
		this.background_color = background_color;
		text_color = AnsiColors.RESET;
	}
}
