using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

class S : Tetrominoes
{
	bool[,] Tetromino;
	S()
	{
		bool[,] Tetromino = 
		{
			{false, true, true},
			{true, true, false},
			{false, false, false},
		};
	}
}

