using Microsoft.Xna.Framework;

class T : Tetromino
{
	public T()
		:base(Color.Magenta)
	{
		block = new bool[,]
		{
			{false, true, false},
			{true, true, true},
			{false, false, false},
		};
	}
}

