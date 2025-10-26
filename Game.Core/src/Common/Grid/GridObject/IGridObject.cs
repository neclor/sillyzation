using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Game.Core.Common.Grid;

public interface IGridObject {

	Vector2I Size { get; set; }
	Vector2I Position { get; set; }

	Grid<Cell>? ParentGrid { get; set; }

	GridCollisionLayer CollisionLayer { get; set; }

	GridCollisionLayer CollisionMask { get; set; }

	bool CanMoveTo(int x, int y);

	bool CanMoveTo(Vector2I position);

	bool CanMoveBy(int dx, int dy);

	bool CanMoveBy(Vector2I delta);

	bool TryMoveTo(int x, int y);

	bool TryMoveTo(Vector2I position);

	bool TryMoveBy(int dx, int dy);

	bool TryMoveBy(Vector2I delta);
}
