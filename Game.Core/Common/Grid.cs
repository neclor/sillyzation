using System.Numerics;

namespace Game.Core;

public partial class Grid<T> {

	public int Width { get; private set; }
	public int Height { get; private set; }
	public Vector2 Size { get; private set; }
	public int Length => _data.Length;

	private T[] _data;

	public T this[int x, int y] {
		get => GetValue(x, y);
		set => SetValue(x, y, value);
	}

#pragma warning disable CA1043
	public T this[Vector2 position] {
		get => this[(int)position.X, (int)position.Y];
		set => this[(int)position.X, (int)position.Y] = value;
	}
#pragma warning restore CA1043

	public Grid(int width, int height) {
		if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width), "Width must be positive.");
		if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height), "Height must be positive.");

		Width = width;
		Height = height;
		Size = new(width, height);
		_data = new T[Width * Height];
	}

	public Grid(int width, int height, Func<T> valueFactory) : this(width, height) {
		ArgumentNullException.ThrowIfNull(valueFactory);
		for (int i = 0; i < _data.Length; i++) {
			_data[i] = valueFactory();
		}
	}

	public Grid(int width, int height, Func<int, int, T> valueFactory) : this(width, height) {
		ArgumentNullException.ThrowIfNull(valueFactory);
		for (int y = 0; y < Height; y++) {
			for (int x = 0; x < Width; x++) {
				_data[IndexFrom(x, y)] = valueFactory(x, y);
			}
		}
	}

	public Grid(Vector2 size) : this((int)size.X, (int)size.Y) { }
	public Grid(Vector2 size, Func<T> valueFactory) : this((int)size.X, (int)size.Y, valueFactory) { }
	public Grid(Vector2 size, Func<int, int, T> valueFactory) : this((int)size.X, (int)size.Y, valueFactory) { }

	public T GetValue(int x, int y) {
		CheckBounds(x, y);
		return _data[IndexFrom(x, y)];
	}

	public T GetValue(Vector2 position) => GetValue((int)position.X, (int)position.Y);

	public void SetValue(int x, int y, T value) {
		CheckBounds(x, y);
		_data[IndexFrom(x, y)] = value;
	}

	public void SetValue(Vector2 position, T value) => SetValue((int)position.X, (int)position.Y, value);

	public void Resize(int width, int height) => InternalResize(width, height);

	public void Resize(int width, int height, Func<T> valueFactory) {
		ArgumentNullException.ThrowIfNull(valueFactory);
		InternalResize(width, height, (x, y) => valueFactory());
	}

	public void Resize(int width, int height, Func<int, int, T> valueFactory) {
		ArgumentNullException.ThrowIfNull(valueFactory);
		InternalResize(width, height, valueFactory);
	}

	public void Resize(Vector2 size) => Resize((int)size.X, (int)size.Y);
	public void Resize(Vector2 size, Func<T> valueFactory) => Resize((int)size.X, (int)size.Y, valueFactory);
	public void Resize(Vector2 size, Func<int, int, T> valueFactory) => Resize((int)size.X, (int)size.Y, valueFactory);

	private static int IndexFrom(int x, int y, int width) => y * width + x;

	private static void CheckValueBounds<TValue>(TValue value, TValue min, TValue max) where TValue : IComparable<TValue> {
		ArgumentOutOfRangeException.ThrowIfLessThan(value, min, nameof(value));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value, max, nameof(value));
	}

	private int IndexFrom(int x, int y) => IndexFrom(x, y, Width);

	private void CheckBounds(int x, int y) {
		CheckXBounds(x);
		CheckYBounds(y);
	}

	private void CheckXBounds(int x) => CheckValueBounds(x, 0, Width - 1);

	private void CheckYBounds(int y) => CheckValueBounds(y, 0, Height - 1);

	private void InternalResize(int width, int height, Func<int, int, T>? valueFactory = null) {
		if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width), "Width must be positive.");
		if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height), "Height must be positive.");

		T[] newData = new T[width * height];

		int minWidth = int.Min(width, Width);
		int minHeight = int.Min(height, Height);

		for (int y = 0; y < minHeight; y++) {
			for (int x = 0; x < minWidth; x++) {
				newData[IndexFrom(x, y, width)] = _data[IndexFrom(x, y)];
			}
		}

		Width = width;
		Height = height;
		Size = new(width, height);
		_data = newData;

		if (width == minWidth || height == minHeight || valueFactory is null) return;

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (x >= minWidth || y >= minHeight) {
					newData[IndexFrom(x, y, width)] = valueFactory(x, y);
				}
			}
		}
	}
}
