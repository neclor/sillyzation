namespace CoreLogic;

internal class Infantry<TCellKey> : Unit<TCellKey> where TCellKey : notnull {
	public Infantry(PlayerKey owner)
		: base(
			"Infantry Division", // Name
			50, // Health
			1, // Speed
			owner
		) { }
}

internal class Tank<TCellKey> : Unit<TCellKey> where TCellKey : notnull {
	public Tank(PlayerKey owner)
		: base(
			"Infantry Division", // Name
			200, // Health
			2, // Speed
			owner
		) { }
}
