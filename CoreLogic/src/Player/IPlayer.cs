namespace CoreLogic;

public record PlayerInit(string name, Color color);

public enum Color {
	Red,
	Orange,
	Yellow,
	LightGreen,
	GREEN,
	LightBlue,
	Blue,
	Purple,
	White,
	Gray,
}

public interface IPlayer {
	uint id { get; }
	string name { get; }
	Color color { get; }
};
