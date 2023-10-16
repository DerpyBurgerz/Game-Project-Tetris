using Microsoft.Xna.Framework;
class Z : Tetromino
{
	public Z()
		:base(Color.Red)
	{
		block = new bool[,]
		{
			{true, true, false},
			{false, true, true},
			{false, false, false},
		};
	}
}

