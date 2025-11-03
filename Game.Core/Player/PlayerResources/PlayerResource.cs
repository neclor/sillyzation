namespace Game.Core.Resources;

public class PlayerResource {

	public int Amount {
		get;
		set => field = int.Max(0, field + value);
	}

	public int Income { get; set; }

	public void Update() => Amount += Income;
}
