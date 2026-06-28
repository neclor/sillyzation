using ErrorOr;
using QuikGraph;
using QuikGraph.Algorithms;

namespace CoreLogic;

internal class Map<TCellKey> where TCellKey : notnull {
	// Stores the keys inside a graph
	private readonly UndirectedGraph<TCellKey, TaggedEdge<TCellKey, uint>> graph;
	// Stores a dictionary of both key and Cells
	private readonly Dictionary<TCellKey, ICell<TCellKey>> cells;
	// Way for algorithm to use the weight
	private readonly Func<TaggedEdge<TCellKey, uint>, double> edgeWeights
		= new(edge => edge.Tag);

	public Map(
		IEnumerable<(TCellKey key, ICell<TCellKey> cell)> cells,
		IEnumerable<(TCellKey key1, TCellKey key2)> connexions,
		IEnumerable<(uint playerId, TCellKey[] cells)> ownerships
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

		foreach ((TCellKey key1, TCellKey key2) in connexions) {
			uint weight = calculateConnexionWeigth(this.cells[key1])
				+ calculateConnexionWeigth(this.cells[key2]);

			if (!graph.AddEdge(new(key1, key2, weight))) {
				throw new InvalidOperationException(
					$"The cells {key1} and {key2} cannot be connected"
				);
			}
		}

		foreach ((uint owner, TCellKey[] owned_cells) in ownerships) {
			if (owned_cells.Length == 0) {
				throw new InvalidOperationException($"Invalid Game State\nA Player has no Starting owned provinces");
			}
			foreach (TCellKey owned in owned_cells) {
				if (!this.cells.TryGetValue(owned, out ICell<TCellKey>? cell)) {
					throw new InvalidOperationException($"Invalid Game State\nCell {owned} does not exist");
				}
				if (cell.owner != null) {
					throw new InvalidOperationException("Invalid Game State\nCell " + owned + " has two owner " + cell.owner + " and " + owner);
				}
				cell.owner = owner;
			}
		}
	}

	private static uint calculateConnexionWeigth(ICell<TCellKey> cell) {
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

	public ErrorOr<ICell<TCellKey>> getCell(TCellKey key) {
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

	public IEnumerable<(TCellKey key, ICell<TCellKey> cell)> getNeightbours(TCellKey key) {
		foreach (var edge in graph.AdjacentEdges(key)) {
			TCellKey neightbour = edge.Source.Equals(key)
				? edge.Target
				: edge.Source;

			yield return (neightbour, getCell(neightbour).Value);
		}
	}

	public ErrorOr<IEnumerable<ICell<TCellKey>>> getShortestPath(TCellKey origin, TCellKey destination) {
		var algorithm = graph.ShortestPathsDijkstra(edgeWeights, origin);
		if (!algorithm(destination, out var path)) {
			return Error.NotFound("Path does not exist");
		}
		return path.Select(step => cells[step.Target]).ToList();
	}
}
