namespace CoreLogic;

public enum Color {
	Red,
	Gold,
	Orange,
	Yellow,
	LightGreen,
	Green,
	LightBlue,
	Blue,
	Purple,
	White,
	Gray,
	Brown,
}

public interface IPlayer {
	PlayerKey id { get; }
	string name { get; }
	Color color { get; }
};
