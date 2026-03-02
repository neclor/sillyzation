using ErrorOr;
using QuikGraph;
using QuikGraph.Algorithms;

namespace CoreLogic;

internal class Map {
	// Stores the keys inside a graph
	private readonly UndirectedGraph<CellKey, TaggedEdge<CellKey, uint>> graph;
	// Stores a dictionary of both key and Cells
	private readonly Dictionary<CellKey, ICell> cells;
	// Way for algorithm to use the weight
	private readonly Func<TaggedEdge<CellKey, uint>, double> edgeWeights
		= new(edge => edge.Tag);

	public Map(
		IEnumerable<(CellKey key, ICell cell)> cells,
		IEnumerable<(CellKey key1, CellKey key2)> connexions
	) {
		ArgumentNullException.ThrowIfNull(cells);
		ArgumentNullException.ThrowIfNull(connexions);

		this.cells = [];
		graph = new();

		foreach ((var key, var cell) in cells) {
			this.cells.Add(key, cell);
			_ = graph.AddVertex(key);
		}

		foreach ((CellKey key1, CellKey key2) in connexions) {
			uint weight = calculateConnexionWeigth(this.cells[key1])
				+ calculateConnexionWeigth(this.cells[key2]);

			if (!graph.AddEdge(new(key1, key2, weight))) {
				throw new InvalidOperationException(
					$"The cells {key1} and {key2} cannot be connected"
				);
			}
		}
	}

	private static uint calculateConnexionWeigth(ICell cell) {
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

	public ErrorOr<ICell> getCell(CellKey key) {
		try {
			return cells[key].ToErrorOr();
		}
		catch (ArgumentNullException) {
			return Error.Unexpected("Invalid Value");
		}
		catch (KeyNotFoundException) {
			return Error.NotFound("Cell not found");
		}
	}

	public IEnumerable<(CellKey key, ICell cell)> getNeightbours(CellKey key) {
		foreach (var edge in graph.AdjacentEdges(key)) {
			CellKey neightbour = edge.Source.Equals(key)
				? edge.Target
				: edge.Source;

			yield return (neightbour, getCell(neightbour).Value);
		}
	}

	public ErrorOr<IEnumerable<ICell>> getShortestPath(CellKey origin, CellKey destination) {
		var algorithm = graph.ShortestPathsDijkstra(edgeWeights, origin);
		if (!algorithm(destination, out var path)) {
			return Error.NotFound("Path does not exist");
		}
		return path.Select(step => cells[step.Target]).ToList();
	}
}
