using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class I : Tetrominoes
{
	public I()
		:base(Color.LightBlue)
	{
		Tetromino = new bool[,]
		{
			{false, false, false, false },
			{true, true, true, true },
			{false, false, false, false },
			{false, false, false, false },
		};
		Width = 4;
	}
	public void draw(SpriteBatch spriteBatch)
	{
		base.draw(spriteBatch);
	}
}
