using QuikGraph;

namespace CoreLogic;

public record MapCell<T>(GameCell Cell, T Key);

public class Map<T> {
	private readonly UndirectedGraph<MapCell<T>, Edge<MapCell<T>>> map = new();
}
