using Microsoft.Xna.Framework;

class L : Tetromino
{
	public L()
		:base(Color.Orange)
	{
		block = new bool[,]
		{
			{true, true, false},
			{false, true, false},
			{false, true, false},
		};
		baseRotationBlock = block;
	}
}

