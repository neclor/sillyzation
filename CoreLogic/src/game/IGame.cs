namespace CoreLogic;

public interface IGame {
	IEnumerable<IPlayer> players { get; }
};