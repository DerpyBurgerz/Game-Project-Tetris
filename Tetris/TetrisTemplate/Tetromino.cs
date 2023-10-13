﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Tetromino
{
	Vector2 position;
	Texture2D cell;
	Color color;
	public bool[,] tetromino;

	int horizontalIndex;
	int verticalIndex;
	public Tetromino(Color color)
	{
		this.color = color;
		cell = TetrisGame.ContentManager.Load<Texture2D>("block");
	}
	public void Input(InputHelper inputhelper)
	{

	}
	public void Collision(string[,] grid)
	{

	}
    public void Collision(Color[,] grid, int x, int y)
    {
        verticalIndex += y;
        horizontalIndex += x;
    }
	public void Reset()
	{

	}
	public void Draw(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < tetromino.GetLength(0); i++)
		{
			for (int j = 0; j < tetromino.GetLength(0); j++)
			{
				if (tetromino[i, j] == true)
				{
					position = new Vector2((j+horizontalIndex) * cell.Width, (i+verticalIndex) * cell.Height);
					spriteBatch.Draw(cell, position, this.color);
				}
			}
		}
	}
	public Color Color { get { return color; } }
	public bool[,] Block {  get { return tetromino; } }
}

