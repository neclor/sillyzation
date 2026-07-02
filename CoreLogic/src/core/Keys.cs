namespace CoreLogic;

#pragma warning disable CA2225 // Operator overloads have named alternates

public readonly record struct PlayerKey(uint value) {
	public static PlayerKey operator ++(PlayerKey key) => new(key.value + 1);
	public static PlayerKey operator --(PlayerKey key) => new(key.value - 1);
	public static implicit operator PlayerKey(uint rawValue) => new(rawValue);
	public override string ToString() => $"PlayerKey({value})";
};

public readonly record struct UnitKey(uint value) {
	public static UnitKey operator ++(UnitKey key) => new(key.value + 1);
	public static UnitKey operator --(UnitKey key) => new(key.value - 1);
	public static implicit operator UnitKey(uint rawValue) => new(rawValue);
	public override string ToString() => $"UnitKey({value})";
};

public readonly record struct QueueKey(uint value) {
	public static QueueKey operator ++(QueueKey key) => new(key.value + 1);
	public static QueueKey operator --(QueueKey key) => new(key.value - 1);
	public static implicit operator QueueKey(uint rawValue) => new(rawValue);
	public override string ToString() => $"QueueKey({value})";
};

#pragma warning restore CA2225 // Operator overloads have named alternates
