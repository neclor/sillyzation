using ErrorOr;
using QuikGraph;
using QuikGraph.Algorithms;

namespace CoreLogic;

internal class Map<CellKey> {
	// Stores the keys inside a graph
	private readonly UndirectedGraph<CellKey, TaggedEdge<CellKey, uint>> graph;
	// Stores a dictionary of both key and Cells
	private readonly Dictionary<CellKey, ICell<CellKey>> cells;
	// Way for algorithm to use the weight
	private readonly Func<TaggedEdge<CellKey, uint>, double> edgeWeights
		= new(edge => edge.Tag);

	public Map(
		IEnumerable<(CellKey key, ICell<CellKey> cell)> cells,
		IEnumerable<(CellKey key1, CellKey key2)> connexions,
		IEnumerable<(uint playerId, CellKey[] cells)> ownerships
	) {
		ArgumentNullException.ThrowIfNull(cells);
		ArgumentNullException.ThrowIfNull(connexions);
		ArgumentNullException.ThrowIfNull(ownerships);

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

		foreach ((uint owner, CellKey[] owned_cells) in ownerships) {
			if (owned_cells.Length == 0) {
				throw new InvalidOperationException($"Invalid Game State\nA Player has no Starting owned provinces");
			}
			foreach (CellKey owned in owned_cells) {
				if (!this.cells.TryGetValue(owned, out ICell<CellKey>? cell)) {
					throw new InvalidOperationException($"Invalid Game State\nCell {owned} does not exist");
				}
				if (cell.owner != null) {
					throw new InvalidOperationException("Invalid Game State\nCell " + owned + " has two owner " + cell.owner + " and " + owner);
				}
				cell.owner = owner;
			}
		}
	}

	private static uint calculateConnexionWeigth(ICell<CellKey> cell) {
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

	public ErrorOr<ICell<CellKey>> getCell(CellKey key) {
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

	public IEnumerable<(CellKey key, ICell<CellKey> cell)> getNeightbours(CellKey key) {
		foreach (var edge in graph.AdjacentEdges(key)) {
			CellKey neightbour = edge.Source.Equals(key)
				? edge.Target
				: edge.Source;

			yield return (neightbour, getCell(neightbour).Value);
		}
	}

	public ErrorOr<IEnumerable<ICell<CellKey>>> getShortestPath(CellKey origin, CellKey destination) {
		var algorithm = graph.ShortestPathsDijkstra(edgeWeights, origin);
		if (!algorithm(destination, out var path)) {
			return Error.NotFound("Path does not exist");
		}
		return path.Select(step => cells[step.Target]).ToList();
	}
}
