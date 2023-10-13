using Microsoft.Xna.Framework;

class L : Tetrominoes
{
	L()
		:base(Color.Orange)
	{
		Tetromino = new bool[,]
		{
			{false, false, true},
			{true, true, true},
			{false, false, false},

		};
	}
}

