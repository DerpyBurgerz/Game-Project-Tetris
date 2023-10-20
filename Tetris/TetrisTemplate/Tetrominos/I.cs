using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class I : Tetromino
{
	public I()
		:base(Color.CornflowerBlue)
	{
		block = new bool[,]
		{
			{false, false, false, false },
			{true, true, true, true },
			{false, false, false, false },
			{false, false, false, false },
		};
	}
}
