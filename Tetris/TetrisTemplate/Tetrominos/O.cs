using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TetrisTemplate.Blocks
{
	internal class I : Tetrominos
	{
		bool[,] Tetromino;
		I()
		{
			bool[,] Tetromino = 
			{
				{false, true, false, false },
				{false, true, false, false },
				{false, true, false, false },
				{false, true, false, false },
			};
		}
	}
}
