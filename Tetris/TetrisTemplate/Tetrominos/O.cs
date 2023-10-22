using Microsoft.Xna.Framework;

class O : Tetromino
{
	public O()
		:base(Color.Yellow)
	{
		block = new bool[,]
		{
			{false, false, false, false },
			{false, true, true, false },
			{false, true, true, false },
			{false, false, false, false },
		};
	}
}

