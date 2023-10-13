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
	public Color[,] grid;

	

    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = Vector2.Zero;
		grid = new Color[Height, Width];
        reset();
        Clear();
    }

    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    
    public void reset()
    {
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				grid[i, j] = Color.White;
			}
		}
	}
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
		for (int i = 0;i < Height;i++)
		{
			for (int j = 0;j < Width; j++)
			{
				position = new Vector2(j*emptyCell.Width, i*emptyCell.Height);
				spriteBatch.Draw(emptyCell, position, grid[i,j]);
			}
		}
    }

    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
    }
}

