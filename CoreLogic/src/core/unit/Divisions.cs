namespace CoreLogic;

internal class Infantry<TCellKey> : Unit<TCellKey> where TCellKey : notnull {
	public Infantry(PlayerKey owner)
		: base(
			UnitType.Infantry,
			"Infantry Division", // Name
			50, // Health
			1, // Speed
			owner
		) { }
}

internal class Tank<TCellKey> : Unit<TCellKey> where TCellKey : notnull {
	public Tank(PlayerKey owner)
		: base(
			UnitType.Tank,
			"Tank Division", // Name
			200, // Health
			2, // Speed
			owner
		) { }
}

internal class Artillery<TCellKey> : Unit<TCellKey> where TCellKey : notnull {
	public Artillery(PlayerKey owner)
		: base(
			UnitType.Artillery,
			"Artillery Division", // Name
			200, // Health
			1, // Speed
			owner
		) { }
}

