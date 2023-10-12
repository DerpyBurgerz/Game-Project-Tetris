using Microsoft.Xna.Framework;

class T : Tetrominoes
{
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

