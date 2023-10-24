using Microsoft.Xna.Framework;

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
		baseRotationBlock = block;
	}
}
