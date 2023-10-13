using Microsoft.Xna.Framework;
class Z : Tetrominoes
{
	public Z()
		:base(Color.Red)
	{
		Tetromino = new bool[,]
		{
			{true, true, false},
			{false, true, true},
			{false, false, false},
		};
	}
}

