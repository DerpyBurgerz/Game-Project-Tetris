using Microsoft.Xna.Framework;
class Z : Tetrominoes
{
	Z()
		:base(Color.Red)
	{
		bool[,] Tetromino = 
		{
			{true, true, false},
			{false, true, true},
			{false, false, false},
		};
	}
}

