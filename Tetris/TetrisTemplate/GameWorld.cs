using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
	Tetrominoes tetrominoe;
	I I;

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
    Tetrominoes blocks;

    public GameWorld()
    {
        newBlock = true;
        random = new Random();
        gameState = GameState.Playing;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
		I = new I();

    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        grid.Draw(gameTime, spriteBatch);
        //spriteBatch.DrawString(font, "Hello!", Vector2.Zero, Color.Blue);
        spriteBatch.End();
    }

    public void Reset()
    {
    }

}
