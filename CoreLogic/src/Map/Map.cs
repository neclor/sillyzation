using QuikGraph;

namespace CoreLogic;

public record MapCell<T>(GameCell cell, T key);

public class Map<T> {
	private readonly UndirectedGraph<MapCell<T>, Edge<MapCell<T>>> map = new();
}
