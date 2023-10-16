using Microsoft.Xna.Framework;
class S : Tetromino
{
	public S()
		:base(Color.Green)
	{
		block = new bool[,]
		{
			{false, true, true},
			{true, true, false},
			{false, false, false},
		};
	}
}

