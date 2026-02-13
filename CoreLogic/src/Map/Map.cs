using QuikGraph;

namespace CoreLogic;

public record MapCell<TKey>(GameCell cell, TKey key);

public class Map<TKey> where TKey : notnull {
	private readonly UndirectedGraph<TKey, Edge<TKey>> graph;
	private readonly Dictionary<TKey, GameCell> cells;

	public Map(
		IEnumerable<(TKey key, GameCell cell)> cells,
		IEnumerable<(TKey key1, TKey key2)> connexions
	) {
		ArgumentNullException.ThrowIfNull(cells);
		ArgumentNullException.ThrowIfNull(connexions);

		this.cells = [];
		graph = new();

		foreach ((TKey key, GameCell cell) in cells) {
			this.cells.Add(key, cell);
			_ = graph.AddVertex(key);
		}

		foreach ((TKey key1, TKey key2) in connexions) {
			if (!graph.AddEdge(new Edge<TKey>(key1, key2))) {
				throw new InvalidOperationException(
					$"The cells {key1} and {key2} cannot be connected"
				);
			}
		}
	}

	public GameCell getCell(TKey key) => cells[key];

	public IEnumerable<(TKey key, GameCell cell)> getNeightbours(TKey key) {
		foreach (Edge<TKey> edge in graph.AdjacentEdges(key)) {
			TKey neightbour = edge.Source.Equals(key)
				? edge.Target
				: edge.Source;

			yield return (neightbour, getCell(neightbour));
		}
	}
}
