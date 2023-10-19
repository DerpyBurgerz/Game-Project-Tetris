﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

class Tetromino
{
	Vector2 position;
	Texture2D cell;
	Color color;
	protected bool[,] block;
	bool[,] tempBlock;

	int horizontalIndex, verticalIndex;
	int newPositionX, newPositionY;
	bool possiblePosition;
	public Tetromino(Color color)
	{
		this.color = color;
		cell = TetrisGame.ContentManager.Load<Texture2D>("block");
		//de horizontalIndex en verticalIndex worden hier op de positie gezet waar de tetromino zichtbaar is als het in de upcomingTetrominos lijst zit in GameWorld.
		//Zodra de tetromino de huidige Tetromino wordt, wordt de Reset method aangeroepen die de tetromino in het speelveld zet.
		horizontalIndex = 11;
		verticalIndex = 8;
	}
    public bool Collision(Color[,] grid, Vector2 movement, bool[,] block)
		//De Collision method checkt of de nieuwe orientatie en positie van de tetromino mogelijk is. 
		//Als het mogelijk is, wordt de tetromino verplaatst, en returnt het true.
		//Als het niet mogelijk is, return het false
    {
		possiblePosition = true;

		for (int i = 0; i < block.GetLength(0); i++)
		{
			for (int j = 0; j < block.GetLength(1); j++)
			{
				newPositionX = i + (int)movement.X + horizontalIndex;
				newPositionY = j + (int)movement.Y + verticalIndex;
				if (block[i, j])
				{
					//Deze if statement checkt of de positie waar de tetromino heen wil gaan niet buiten het speelveld ligt.
					if ((newPositionX < 0 || newPositionX >=grid.GetLength(0)) || (newPositionY < 0 || newPositionY >= grid.GetLength(1)))
					{
						possiblePosition = false;
					}
					//Deze if statement checkt of de tetromino overlapt met een gekleurd vakje in de grid.
					else if (grid[newPositionX, newPositionY] != Color.White)
					{
						possiblePosition = false;
					}
				}
			}
		}
		if (possiblePosition)
		{
			verticalIndex += (int)movement.Y;
			horizontalIndex += (int)movement.X;
			return true;
		}
		return false;
	}

    
    public void Rotate(Color[,] grid)//de Rotate method draait de Tetromino als de Tetromino kan draaien.
	{
		//source: https://stackoverflow.com/questions/646468/how-to-rotate-a-2d-array-of-integers

		tempBlock = new bool[block.GetLength(0), block.GetLength(0)];
		for (int i = (block.GetLength(0) - 1); i >= 0; --i)
        {
            for (int j = 0; j < block.GetLength(0); ++j)
            {
			
				tempBlock[j, block.GetLength(0) -1 - i] = block[i, j];
            }
        }
		if (Collision(grid, new Vector2(0,0) , tempBlock))
			block = tempBlock;
    }

    
    public void Reset()//Deze method zet de Tetromino in het speelveld
	{
		verticalIndex = 0;
		horizontalIndex = 0;
	}
	public void Draw(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < block.GetLength(0); i++)
		{
			for (int j = 0; j < block.GetLength(1); j++)
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
	public bool[,] Block { get { return block; } }
	public int VerticalIndex { get { return verticalIndex; } }
	public int HorizontalIndex { get { return horizontalIndex; } }
}

