using Microsoft.Xna.Framework;

class J : Tetromino
{
	public J()
		:base(Color.DarkBlue)
	{
		block = new bool[,]
		{
			{false, true, false},
			{false, true, false},
			{true, true, false},
		};
	}
}

