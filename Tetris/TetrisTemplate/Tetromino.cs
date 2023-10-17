using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Tetromino
{
	Vector2 position;
	Texture2D cell;
	Color color;
	public bool[,] block;

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
    public void Collision(Color[,] grid, Vector2 movement)
    {
		for (int i = 0; i < block.GetLength(0); i++)
		{
			for (int j = 0; j < block.GetLength(0); j++)
			{
				if (grid[i+(int)movement.X+horizontalIndex, j+(int)movement.Y+verticalIndex] != Color.White)
				{
					break;
				}
			}
		}
		verticalIndex += (int)movement.Y;
		horizontalIndex += (int)movement.X;
	}
	public void Reset()
	{
		verticalIndex = 0;
		horizontalIndex = 0;
	}
	public void Draw(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < block.GetLength(0); i++)
		{
			for (int j = 0; j < block.GetLength(0); j++)
			{
				if (block[i, j] == true)
				{
					position = new Vector2((i+horizontalIndex) * cell.Width, (j+verticalIndex) * cell.Height);
					spriteBatch.Draw(cell, position, this.color);
				}
			}
		}
	}
	public Color Color { get { return color; } }
	public bool[,] Block {  get { return block; } }
	public int VerticalIndex { get { return verticalIndex; } }
	public int HorizontalIndex { get { return horizontalIndex; } }
}

