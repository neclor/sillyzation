using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Core.Common.Grid;

public class Cell {

	public IReadOnlyList<IGridObject> GridObjects => _gridObjects;

	private readonly List<IGridObject> _gridObjects = [];

	public bool AddGridObject(IGridObject gridObject) {
		if (_gridObjects.Contains(gridObject)) return false;

		_gridObjects.Add(gridObject);
		return true;
	}

	public void ClearGridObjects() => _gridObjects.Clear();

	public bool ContainsGridObject(IGridObject gridObject) => _gridObjects.Contains(gridObject);

	public bool RemoveGridObject(IGridObject gridObject) => _gridObjects.Remove(gridObject);
}
