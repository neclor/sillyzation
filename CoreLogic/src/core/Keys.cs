namespace CoreLogic;

#pragma warning disable CA2225 // Operator overloads have named alternates

public readonly record struct PlayerKey(uint value) {
	public static PlayerKey operator ++(PlayerKey key) => new(key.value + 1);
	public static PlayerKey operator --(PlayerKey key) => new(key.value - 1);
	public static implicit operator PlayerKey(uint rawValue) => new(rawValue);
};

public readonly record struct UnitKey(uint value) {
	public static UnitKey operator ++(UnitKey key) => new(key.value + 1);
	public static UnitKey operator --(UnitKey key) => new(key.value - 1);
	public static implicit operator UnitKey(uint rawValue) => new(rawValue);
};

public readonly record struct QueueKey(uint value) {
	public static QueueKey operator ++(QueueKey key) => new(key.value + 1);
	public static QueueKey operator --(QueueKey key) => new(key.value - 1);
	public static implicit operator QueueKey(uint rawValue) => new(rawValue);
};

public readonly record struct FrontKey(uint value) {
	public static FrontKey operator ++(FrontKey key) => new(key.value + 1);
	public static FrontKey operator --(FrontKey key) => new(key.value - 1);
	public static implicit operator FrontKey(uint rawValue) => new(rawValue);
};

#pragma warning restore CA2225 // Operator overloads have named alternates
