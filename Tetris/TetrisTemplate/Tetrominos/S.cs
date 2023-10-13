using Microsoft.Xna.Framework;
class S : Tetrominoes
{
	S()
		:base(Color.Green)
	{
		Tetromino = new bool[,]
		{
			{false, true, true},
			{true, true, false},
			{false, false, false},
		};
	}
}

