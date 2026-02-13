using QuikGraph;

namespace CoreLogic;

public record MapCell<T>(GameCell cell, T key);

public class Map<T> where T : notnull {
	private readonly UndirectedGraph<T, Edge<T>> graph;
	private readonly Dictionary<T, GameCell> cells;

	public Map(
		IEnumerable<(T key, GameCell cell)> cells,
		IEnumerable<(T key1, T key2)> connexions
	) {
		ArgumentNullException.ThrowIfNull(cells);
		ArgumentNullException.ThrowIfNull(connexions);

		this.cells = [];
		graph = new();

		foreach ((T key, GameCell cell) in cells) {
			this.cells.Add(key, cell);
			_ = graph.AddVertex(key);
		}

		foreach ((T key1, T key2) in connexions) {
			if (!graph.AddEdge(new Edge<T>(key1, key2))) {
				throw new InvalidOperationException(
					$"The cells {key1} and {key2} cannot be connected"
				);
			}
		}
	}

	public GameCell getCell(T key) => cells[key];

	public IEnumerable<(T key, GameCell cell)> getNeightbours(T key) {
		foreach (Edge<T> edge in graph.AdjacentEdges(key)) {
			T neightbour = edge.Source.Equals(key)
				? edge.Target
				: edge.Source;

			yield return (neightbour, getCell(neightbour));
		}
	}
}
