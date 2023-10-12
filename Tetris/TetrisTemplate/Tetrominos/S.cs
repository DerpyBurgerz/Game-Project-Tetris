using Microsoft.Xna.Framework;
class S : Tetrominoes
{
	bool[,] Tetromino;
	S()
		:base(Color.Green)
	{
		bool[,] Tetromino = 
		{
			{false, true, true},
			{true, true, false},
			{false, false, false},
		};
	}
}

