﻿using Microsoft.Xna.Framework;

class J : Tetromino
{
	public J()
		:base(Color.DarkBlue)
	{
		tetromino = new bool[,]
		{
			{true, false, false},
			{true, true, true},
			{false, false, false},
		};
	}
}

