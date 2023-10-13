using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Tetrominoes
{
	Vector2 position;
	Texture2D cell;
	Color color;
	public bool[,] Tetromino;
	public Tetrominoes(Color color)
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
	public void draw(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < Tetromino.GetLength(0); i++)
		{
			for (int j = 0; j < Tetromino.GetLength(0); j++)
			{
				if (Tetromino[i, j] == true)
				{
					position = new Vector2(j * cell.Width, i * cell.Height);
					spriteBatch.Draw(cell, position, this.color);
				}
			}
		}
	}
}

