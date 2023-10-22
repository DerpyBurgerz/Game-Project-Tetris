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
	int totalLinesCleared = 0;
	
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = Vector2.Zero;
		grid = new Color[Width, Height];
        Clear();
    }
    public void Clear()
	//Deze methode vult de grid met empty cells
	{
		for (int i = 0; i < Width; i++)
		{
			for (int j = 0; j < Height; j++)
			{
				grid[i, j] = GameWorld.EmptyCell;
			}
		}
	}
	public void AddToGrid(Color color, int horizontalPosition, int verticalPosition, bool[,] tetromino)
	//De Add methode voegt een Tetromino toe aan de grid
	{
		for (int i = 0; i < tetromino.GetLength(0); i++)
		{
			for (int j = 0; j < tetromino.GetLength(1); j++)
			{
				if (tetromino[j, i])
				{
					grid[j + horizontalPosition, i + verticalPosition] = color;
				}
			}
		}
	}
	public int CheckFullRows()
	//De CheckFullRows methode checkt of er een rij vol is, haalt deze weg en returnt het aantal weggehaalde rijen.
	{
        fullRows = 0;
		for (int i = 1; i < grid.GetLength(1); i++)
		{
			isRowFull = true;
			for (int j = 0; j < grid.GetLength(0); j++)
			{
				        
				if (grid[j, i] == GameWorld.EmptyCell)
				{
					isRowFull = false;  
				}
			}
            if (isRowFull)
            {
                RemoveRow(i);
                fullRows++;
				totalLinesCleared++;
            }
		}
		if (fullRows == 2)
			fullRows = 2 ;
		return fullRows;
	}
	public void RemoveRow(int y)
	{
	//Deze methode haalt een rij y weg
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
	//Deze methode voegt een nieuw rij toe aan de bovenkant van de grid.
    {
        for (int i = 0;  i < grid.GetLength(0); i++)
        {
            grid[i, 0] = GameWorld.EmptyCell;
        }
    }
    
	
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	//De Draw method tekent de grid.
	//Het gebruikt de Color type uit de array als filter over de emptyCell sprite om de gevulde vakjes een kleur te geven.
    {
		for (int i = 0;i < Width;i++)
		{
			for (int j = 0;j < Height; j++)
			{
				position = new Vector2(i*emptyCell.Width + GameWorld.StartingpointGrid.X, j*emptyCell.Height + GameWorld.StartingpointGrid.Y);
				spriteBatch.Draw(emptyCell, position, grid[i,j]);
			}
		}
    }
    public Color[,] Grid { get { return grid; } }
	public int WidthEmptyCell { get { return emptyCell.Width; } }

	public int TotalLinesCleared {  get { return totalLinesCleared; } }
}

