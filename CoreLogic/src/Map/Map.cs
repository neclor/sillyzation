using QuikGraph;

namespace CoreLogic;

internal class Map<TKey> where TKey : notnull, IEquatable<TKey> {
	private record MapCell<T>(ICell<T> cell, T key);

	// Stores the keys inside a graph
	private readonly UndirectedGraph<TKey, Edge<TKey>> graph;
	// Stores a dictionary of both key and Cells
	private readonly Dictionary<TKey, ICell<TKey>> cells;

	public Map(
		IEnumerable<(TKey key, ICell<TKey> cell)> cells,
		IEnumerable<(TKey key1, TKey key2)> connexions
	) {
		ArgumentNullException.ThrowIfNull(cells);
		ArgumentNullException.ThrowIfNull(connexions);

		this.cells = [];
		graph = new();

		foreach ((TKey key, ICell<TKey> cell) in cells) {
			this.cells.Add(key, cell);
			_ = graph.AddVertex(key);
		}

		foreach ((TKey key1, TKey key2) in connexions) {
			if (!graph.AddEdge(new(key1, key2))) {
				throw new InvalidOperationException(
					$"The cells {key1} and {key2} cannot be connected"
				);
			}
		}
	}

	public ICell<TKey> getCell(TKey key) => cells[key];

	public IEnumerable<(TKey key, ICell<TKey> cell)> getNeightbours(TKey key) {
		foreach (Edge<TKey> edge in graph.AdjacentEdges(key)) {
			TKey neightbour = edge.Source.Equals(key)
				? edge.Target
				: edge.Source;

			yield return (neightbour, getCell(neightbour));
		}
	}
}
