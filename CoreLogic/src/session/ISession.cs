using CoreLogic;
using ErrorOr;

namespace session;

internal interface ISession<CellKey> {
	// Give the UI the player to display (will always be the same if you are in
	// multiplayer on the internet or on a singleplayer game)
	IPlayer currentPlayer { get; }

	// Give the current map state
	bool gameState { get; }

	ErrorOr<IPlayer> getPlayer(PlayerKey playerId);
	Dictionary<uint, IPlayer> getAllPlayers();

	ErrorOr<ICell<CellKey>> getCell(uint playerId, CellKey cellId);
}