using Microsoft.Xna.Framework;

class L : Tetrominoes
{
	bool[,] Tetromino;
	L()
		:base(Color.Orange)
	{
		bool[,] Tetromino = 
		{
			{false, false, true},
			{true, true, true},
			{false, false, false},

		};
	}
}

