namespace CoreLogic.Units;

public class BaseUnit(
	uint baseHealth,
	uint health,
	uint speed
) {
	public uint BaseHealth { get; } = baseHealth;
	public uint Health { get; } = health;
	public uint Speed { get; } = speed;
}
