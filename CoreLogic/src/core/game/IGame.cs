using ErrorOr;

namespace CoreLogic;

public interface IGame {
	// Player
	ErrorOr<IPlayer> getPlayer(PlayerKey playerId);
	Dictionary<PlayerKey, IPlayer> getAllPlayers();
	ErrorOr<Success> addPlayer(string name, Color color);
	ErrorOr<Success> kickPlayer(PlayerKey playerId);
};