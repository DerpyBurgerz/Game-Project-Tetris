using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TetrisTemplate.Blocks
{
	internal class J : Tetrominoes
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
}
