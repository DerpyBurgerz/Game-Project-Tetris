using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

class T : Tetrominoes
{
	bool[,] Tetromino;
	T()
	{
		bool[,] Tetromino = 
		{
			{false, true, false},
			{true, true, true},
			{false, false, false},
		};
	}
}

