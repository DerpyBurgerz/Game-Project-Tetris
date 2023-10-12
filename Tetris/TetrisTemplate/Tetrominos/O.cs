using Microsoft.Xna.Framework;

class O : Tetrominoes
{
	O()
		:base(Color.Yellow)
	{
		bool[,] Tetromino = 
		{
			{false, false, false, false },
			{false, true, true, false },
			{false, true, true, false },
			{false, false, false, false },
		};
	}
}

