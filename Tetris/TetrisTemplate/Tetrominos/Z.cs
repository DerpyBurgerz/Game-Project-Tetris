using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TetrisTemplate.Blocks
{
	internal class Z : Tetrominoes
	{
		bool[,] Tetromino;
		Z()
		{
			bool[,] Tetromino = 
			{
				{true, true, false},
				{false, true, true},
				{false, false, false},
			};
		}
	}
}
