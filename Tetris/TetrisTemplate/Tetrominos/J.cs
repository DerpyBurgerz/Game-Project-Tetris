using Microsoft.Xna.Framework;

class J : Tetrominoes
{
	J()
		:base(Color.DarkBlue)
	{
		Tetromino = new bool[,]
		{
			{true, false, false},
			{true, true, true},
			{false, false, false},
		};
	}
}

