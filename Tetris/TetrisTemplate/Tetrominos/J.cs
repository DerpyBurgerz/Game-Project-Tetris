using System;
using Microsoft.Xna.Framework;

class J : Tetrominoes
{
	bool[,] Tetromino;
	J()
	{
		bool[,] Tetromino = 
		{
			{true, false, false},
			{true, true, true},
			{false, false, false},
		};
	}
}

