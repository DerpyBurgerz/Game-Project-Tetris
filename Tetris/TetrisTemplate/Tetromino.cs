﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Tetromino
{
	Vector2 position;
	Texture2D cell;
	Color color;
	public bool[,] tetromino;
	public Tetromino(Color color)
	{
		this.color = color;
		cell = TetrisGame.ContentManager.Load<Texture2D>("block");
	}
	public void Input()
	{

	}
	public void collision(string[,] grid)
	{

	}
	public void reset()
	{

	}
	public void draw(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < tetromino.GetLength(0); i++)
		{
			for (int j = 0; j < tetromino.GetLength(0); j++)
			{
				if (tetromino[i, j] == true)
				{
					position = new Vector2((j+1) * cell.Width, (i+0) * cell.Height);
					spriteBatch.Draw(cell, position, this.color);
				}
			}
		}
	}
}

