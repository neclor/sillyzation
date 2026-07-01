using CoreLogic;
using ErrorOr;

namespace session;

internal interface ISession<TCellKey> where TCellKey : notnull {
	// Give the UI the player to display (will always be the same if you are in
	// multiplayer on the internet or on a singleplayer game)
	PlayerKey currentPlayerId { get; }
	IPlayer currentPlayer { get; }

	// Give the current map state
	bool gameState { get; }

	ErrorOr<IPlayer> getPlayer(PlayerKey playerId);
	Dictionary<PlayerKey, IPlayer> getAllPlayers();

	ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId);

	void endTurn();
}