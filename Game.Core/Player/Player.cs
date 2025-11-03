using System.Drawing;
using Game.Core.Resources;

namespace Game.Core;

public class Player {

	public Guid Id { get; set; } = Guid.NewGuid();

	public string Name { get; set; } = "Player";

	public Color Color { get; set; } = Color.White;

	public PlayerResources Resources { get; } = new();

	public IList<IPlayerObject> PlayerObjects { get; } = [];

	public bool IsAI { get; set; }

	public bool IsReady { get; set; }
}
