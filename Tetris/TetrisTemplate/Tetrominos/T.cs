using Microsoft.Xna.Framework;

class T : Tetrominoes
{
	public T()
		:base(Color.Magenta)
	{
		Tetromino = new bool[,]
		{
			{false, true, false},
			{true, true, true},
			{false, false, false},
		};
	}
}

