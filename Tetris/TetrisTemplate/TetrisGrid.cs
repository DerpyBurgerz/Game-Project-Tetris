using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    /// The sprite of a single empty cell in the grid.
    Texture2D emptyCell;
    Vector2 position;
    /// The number of grid elements in the x-direction.
    static public int Width { get { return 10; } }
   
    /// The number of grid elements in the y-direction.
    static public int Height { get { return 20; } }
	Color[,] grid;
	bool isRowFull;
    int fullRows;
	

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = Vector2.Zero;
		grid = new Color[Width, Height];
        Clear();
    }

    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    
    public void Reset()
    {
		
	}
	public int CheckFullRows()
	{
        fullRows = 0;
		for (int i = 1; i < grid.GetLength(1); i++)
		{
			isRowFull = true;
			for (int j = 0; j < grid.GetLength(0); j++)
			{
				        
				if (grid[j, i] == Color.White)
				{
					isRowFull = false;  
				}
			}
            if (isRowFull)
            {
                RemoveRow(i);
                fullRows++;
            }
		}
		return fullRows;
	}
	public void RemoveRow(int y)
	{
		fullRows++;
		for (int i = 0; i < y; i++)
		{
			for (int j = 0; j < grid.GetLength(0); j++)
			{
				grid[j, y - i] = grid[j, y - i - 1];
			}
		}
		NewRow();
	}
	public void NewRow()
    {
        for (int i = 0;  i < grid.GetLength(0); i++)
        {
            grid[i, 0] = Color.White;
        }
    }
    
	public void Add(Color color, int horizontalPosition, int verticalPosition, bool[,] tetromino)
    {
        for (int i=0; i < tetromino.GetLength(0); i++) 
        { 
            for (int j=0; j< tetromino.GetLength(1); j++)
            {
                if (tetromino[j, i])
                {
                    grid[j+horizontalPosition, i+verticalPosition] = color;
                }
            }
        }
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
		for (int i = 0;i < Width;i++)
		{
			for (int j = 0;j < Height; j++)
			{
				position = new Vector2(i*emptyCell.Width, j*emptyCell.Height);
				spriteBatch.Draw(emptyCell, position, grid[i,j]);
			}
		}
    }

    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				grid[i, j] = Color.White;
			}
		}
	}
    public Color[,] Grid { get { return grid; } }
	public int WidthEmptyCell { get { return emptyCell.Width; } }
}

