using ErrorOr;
using QuikGraph;
using QuikGraph.Algorithms;

namespace CoreLogic;

internal class Map<TKey> where TKey : notnull, IEquatable<TKey> {
	private record MapCell<T>(ICell<T> cell, T key);

	// Stores the keys inside a graph
	private readonly UndirectedGraph<TKey, TaggedEdge<TKey, uint>> graph;
	// Stores a dictionary of both key and Cells
	private readonly Dictionary<TKey, ICell<TKey>> cells;
	// Way for algorithm to use the weight
	private readonly Func<TaggedEdge<TKey, uint>, double> edgeWeights
		= new(edge => edge.Tag);

	public Map(
		IEnumerable<(TKey key, ICell<TKey> cell)> cells,
		IEnumerable<(TKey key1, TKey key2)> connexions
	) {
		ArgumentNullException.ThrowIfNull(cells);
		ArgumentNullException.ThrowIfNull(connexions);

		this.cells = [];
		graph = new();

		foreach ((var key, var cell) in cells) {
			this.cells.Add(key, cell);
			_ = graph.AddVertex(key);
		}

		foreach ((TKey key1, TKey key2) in connexions) {
			uint weight = calculateConnexionWeigth(this.cells[key1])
				+ calculateConnexionWeigth(this.cells[key2]);

			if (!graph.AddEdge(new(key1, key2, weight))) {
				throw new InvalidOperationException(
					$"The cells {key1} and {key2} cannot be connected"
				);
			}
		}
	}

	private static uint calculateConnexionWeigth(ICell<TKey> cell) {
		return cell.terrain switch {
			Terrain.Plain => 1,
			Terrain.Forest => 3,
			Terrain.Desert => 3,
			Terrain.Tundra => 4,
			Terrain.Swamp => 4,
			Terrain.Savanna => 1,
			Terrain.Jungle => 5,
			_ => 10,
		};
	}

	public ICell<TKey> getCell(TKey key) => cells[key];

	public IEnumerable<(TKey key, ICell<TKey> cell)> getNeightbours(TKey key) {
		foreach (var edge in graph.AdjacentEdges(key)) {
			TKey neightbour = edge.Source.Equals(key)
				? edge.Target
				: edge.Source;

			yield return (neightbour, getCell(neightbour));
		}
	}

	public ErrorOr<IEnumerable<ICell<TKey>>> getShortestPath(TKey origin, TKey destination) {
		var algorithm = graph.ShortestPathsDijkstra(edgeWeights, origin);
		if (!algorithm(destination, out var path)) {
			return Error.NotFound("Path does not exist");
		}
		return path.Select(step => cells[step.Target]).ToList();
	}
}
