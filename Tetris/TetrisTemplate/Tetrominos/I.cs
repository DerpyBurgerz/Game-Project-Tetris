using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TetrisTemplate.Blocks
{
	internal class I : Tetrominoes
	{
		bool[,] Tetromino;
		I()
		{
			bool[,] Tetromino = 
			{
				{false, false, false, false },
				{true, true, true, true },
				{false, false, false, false },
				{false, false, false, false },
			};
		}
	}
}
