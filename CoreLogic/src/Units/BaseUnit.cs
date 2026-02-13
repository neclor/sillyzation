namespace CoreLogic.Units;

public class BaseUnit(
	uint baseHealth,
	uint health,
	uint speed
) {
	public uint baseHealth { get; } = baseHealth;
	public uint health { get; } = health;
	public uint speed { get; } = speed;
}
