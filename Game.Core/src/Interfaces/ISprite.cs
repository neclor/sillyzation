using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Game.Core;

public interface ISprite {

	string SpritePath { get; set; }

	Vector2 Scale { get; set; }
	Color Modulate { get; set; }

}
