using System.Drawing;

namespace Shared.Interfaces;

public interface IServerHub {

	Task<(string GameCode, Guid Id)> CreateGameAsync();
	Task<Guid?> JoinGameAsync(string gameCode);
	Task<bool> SendTurnAsync();

	Task UpdatePlayer(Guid playerId, string newName, Color newColor, );
}
