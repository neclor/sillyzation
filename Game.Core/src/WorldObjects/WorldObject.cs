using System.Drawing;
using System.Numerics;
using Game.Core.Common;

namespace Game.Core.WorldObjects;

public abstract class WorldObject : IGridObject, ISprite {

	public Vector2 Position { get; set; }

	public Grid<IGridObject>? ParentGrid { get; set; }

	public string SpritePath { get; set; } = "";

	public Vector2 Scale { get; set; } = Vector2.One;
	public Color Modulate { get; set; } = Color.White;




}
