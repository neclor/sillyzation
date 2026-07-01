using CoreLogic;
using ErrorOr;

namespace session;

internal interface ISession<TCellKey> where TCellKey : notnull {
	PlayerKey currentPlayerId { get; }
	ISessionPlayer currentPlayer { get; }
	IUnit<TCellKey>[] current_player_units { get; }
	bool gameState { get; }

	ErrorOr<ISessionPlayer> getPlayer(PlayerKey playerId);
	Dictionary<PlayerKey, ISessionPlayer> getAllPlayers();

	ErrorOr<ICell<TCellKey>> getCell(PlayerKey playerId, TCellKey cellId);

	void endTurn();
}