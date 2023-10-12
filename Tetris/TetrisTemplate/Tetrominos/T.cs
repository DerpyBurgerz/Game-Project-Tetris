using Microsoft.Xna.Framework;

class T : Tetrominoes
{
	bool[,] Tetromino;
	T()
		:base(Color.Magenta)
	{
		bool[,] Tetromino = 
		{
			{false, true, false},
			{true, true, true},
			{false, false, false},
		};
	}
}

