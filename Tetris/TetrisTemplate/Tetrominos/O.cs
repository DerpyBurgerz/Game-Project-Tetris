using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

class O : Tetrominoes
{
	bool[,] Tetromino;
	O()
	{
		bool[,] Tetromino = 
		{
			{false, false, false, false },
			{false, true, true, false },
			{false, true, true, false },
			{false, false, false, false },
		};
	}
}

