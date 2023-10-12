using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

class L : Tetrominoes
{
	bool[,] Tetromino;
	L()
	{
		bool[,] Tetromino = 
		{
			{false, false, true},
			{true, true, true},
			{false, false, false},

		};
	}
}

