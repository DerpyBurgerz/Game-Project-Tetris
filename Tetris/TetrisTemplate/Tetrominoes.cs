using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

class Tetrominoes
{
	Vector2 position;
	Texture2D cell;
	Color color;
	public bool[,] Tetromino;

	public int horizontalIndex = (TetrisGrid.Width / 2) - 2;
	public int verticalIndex = 2;


	public Tetrominoes(Color color)
	{
		this.color = color;
		cell = TetrisGame.ContentManager.Load<Texture2D>("block");
	}
	public void Input(InputHelper InputHelper)
	{

        if (InputHelper.KeyPressed(Keys.A))
        {
            //horizontalIndex = horizontalIndex - 1;
        }

        if (InputHelper.KeyPressed(Keys.D))
        {
            horizontalIndex = horizontalIndex + 1;
        }

        if (InputHelper.KeyPressed(Keys.S))
        {
            verticalIndex = verticalIndex + 1;
        }
    }
	public void Collision(Color[,] grid, int x, int y)
	{
		verticalIndex += y;
		horizontalIndex += x;
	}
	public void draw(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < Tetromino.GetLength(0); i++)
		{
			for (int j = 0; j < Tetromino.GetLength(0); j++)
			{
				if (Tetromino[i, j] == true)
				{
					position = new Vector2((j + horizontalIndex) * cell.Width, (i + verticalIndex) * cell.Height);
					spriteBatch.Draw(cell, position, this.color);
				}
			}
		}
	}
}

