using Microsoft.Xna.Framework;
class S : Tetromino
{
	public S()
		:base(Color.LimeGreen)
	{
		block = new bool[,]
		{
			{true, false, false},
			{true, true, false},
			{false, true, false},
		};
	}
}

