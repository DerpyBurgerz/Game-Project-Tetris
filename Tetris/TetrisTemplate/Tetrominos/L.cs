using Microsoft.Xna.Framework;

class L : Tetromino
{
	public L()
		:base(Color.Orange)
	{
		block = new bool[,]
		{
			{false, false, true},
			{true, true, true},
			{false, false, false},

		};
	}
}

