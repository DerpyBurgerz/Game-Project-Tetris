using Microsoft.Xna.Framework;
class Z : Tetromino
{
	public Z()
		:base(Color.Red)
	{
		block = new bool[,]
		{
			{false, true, false},
			{true, true, false},
			{true, false, false},
		};
	}
}

