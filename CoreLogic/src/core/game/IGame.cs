using ErrorOr;

namespace CoreLogic;

public interface IGame {
	// Player
	ErrorOr<IPlayer> getPlayer(PlayerKey playerId);
	Dictionary<PlayerKey, IPlayer> getAllPlayers();
	ErrorOr<bool> addPlayer(string name, Color color);
	ErrorOr<bool> kickPlayer(PlayerKey playerId);
};