using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
//using TetrisTemplate;

/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    enum GameState
    {
        Playing,
        GameOver
    }

    bool newBlock;
	Tetromino tetromino;

    /// <summary>
    /// The random-number generator of the game.
    /// </summary>
    public static Random Random { get { return random; } }
    static Random random;

    /// <summary>
    /// The main font of the game.
    /// </summary>
    SpriteFont font;

    /// <summary>
    /// The current game state.
    /// </summary>
    GameState gameState;

    /// <summary>
    /// The main grid of the game.
    /// </summary>
    TetrisGrid grid;
    IDictionary<Keys, Vector2> movement;

	public GameWorld()
    {
        newBlock = true;
        random = new Random();
        gameState = GameState.Playing;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
        tetromino = new I();
        movement = new Dictionary<Keys, Vector2>()
        {
            {Keys.A, new Vector2(-1, 0) },
            {Keys.D, new Vector2(1, 0) },
            {Keys.S, new Vector2(0, 1) },
            //{Keys.W, new Vector2(0, -1) },
        };
	}

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        //
        foreach (Keys key in movement.Keys)
            if (inputHelper.KeyPressed(key)) 
                tetromino.Collision(grid.grid, movement[key]);
        /*if (inputHelper.KeyPressed(Keys.A))
        {
            tetromino.Collision(grid.grid, -1, 0);
        }
        if (inputHelper.KeyPressed(Keys.D))
        {
            tetromino.Collision(grid.grid, 1, 0);
        }
        if (inputHelper.KeyPressed(Keys.S))
        {
            tetromino.Collision(grid.grid, 0, 1);
        }
		if (inputHelper.KeyPressed(Keys.W))
		{
			tetromino.Collision(grid.grid, 0, -1);
		}
        */
    }

    public void Update(GameTime gameTime)
    {
        //grid.Add(tetromino.Color, new Vector2(5, 4), tetromino.Block);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        grid.Draw(gameTime, spriteBatch);
		tetromino.Draw(spriteBatch);
        //spriteBatch.DrawString(font, "Hello!", Vector2.Zero, Color.Blue);
        spriteBatch.End();
    }

    public void Reset()
    {
        tetromino.Reset();
        grid.Clear();
    }

}
