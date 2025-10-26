using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Game.Core.Common.Grid;

private class InternalGridObject(IGridObject owner) : IGridObject {

	private readonly IGridObject _owner = owner;


	Vector2I Position {
		get;
		set {







		}
	}

	Grid<Cell>? ParentGrid { get; set; }



	bool CanMoveTo(int x, int y);

	bool CanMoveBy(int dx, int dy);

	bool TryMoveTo(int x, int y);

	bool TryMoveBy(int dx, int dy);




}
