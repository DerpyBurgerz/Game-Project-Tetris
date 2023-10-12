using Microsoft.Xna.Framework;
class I : Tetrominoes
{
	public I()
		:base(Color.LightBlue)
	{
		bool[,] Tetromino = 
		{
			{false, false, false, false },
			{true, true, true, true },
			{false, false, false, false },
			{false, false, false, false },
		};
		Width = 4;
	}	
}
