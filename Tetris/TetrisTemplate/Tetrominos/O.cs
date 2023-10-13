using Microsoft.Xna.Framework;

class O : Tetrominoes
{
	public O()
		:base(Color.Yellow)
	{
		Tetromino = new bool[,]
		{
			{false, false, false, false },
			{false, true, true, false },
			{false, true, true, false },
			{false, false, false, false },
		};
	}
}

