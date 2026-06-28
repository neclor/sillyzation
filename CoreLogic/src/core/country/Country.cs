namespace CoreLogic;

public class Country {
	public static readonly Country England = new("England", Color.Red);
	public static readonly Country France = new("France", Color.Blue);
	public static readonly Country Germany = new("Germany", Color.Gray);
	public static readonly Country Russia = new("Russia", Color.DarkGreen);
	public static readonly Country Italy = new("Italy", Color.Green);
	public static readonly Country Spain = new("Spain", Color.Yellow);
	public static readonly Country Belgium = new("Belgium", Color.Gold);
	public static readonly Country Netherland = new("Netherland", Color.Orange);
	public static readonly Country AustriaHungary = new("Austria-Hungary", Color.Brown);
	public static readonly Country Ottoman = new("Ottoman", Color.LightGreen);

	public string name { get; }
	public Color color { get; }

	private Country(string name, Color color) {
		this.name = name;
		this.color = color;
	}
}