using Microsoft.Xna.Framework;

class J : Tetrominoes
{
	bool[,] Tetromino;
	J()
		:base(Color.DarkBlue)
	{
		bool[,] Tetromino = 
		{
			{true, false, false},
			{true, true, true},
			{false, false, false},
		};
	}
}

