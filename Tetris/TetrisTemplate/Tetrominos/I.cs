using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class I : Tetromino
{
	public I()
		:base(Color.CornflowerBlue)
	{
		block = new bool[,]
		{
			{false, true, false, false },
			{false, true, false, false },
			{false, true, false, false },
			{false, true, false, false },
		};
	}
}
