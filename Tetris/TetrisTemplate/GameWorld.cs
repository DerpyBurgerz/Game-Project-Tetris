using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
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
        GameOver,
        Startup
    }

	Tetromino tetromino;
    List <Tetromino> upcomingTetrominos, bagOfTetrominos, newBag;

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
    IDictionary<Keys, Vector2> direction;

	public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
        tetromino = new I();
        gameState = GameState.Startup;
        //In deze dictionary staan de toetsen die je in kan drukken voor de beweging van de tetromino's, en de beweging die het doet als je die toets indrukt.
        direction = new Dictionary<Keys, Vector2>()
        {
            {Keys.A, new Vector2(-1, 0) },
            {Keys.D, new Vector2(1, 0) },
            {Keys.S, new Vector2(0, 1) },
            {Keys.W, new Vector2(0, -1) },
        };

        upcomingTetrominos = new List<Tetromino>();
        newBag = new List<Tetromino>();
        bagOfTetrominos = new List<Tetromino>();
        upcomingTetrominos.AddRange(AddBag());
	}

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        //Dit loopt door de dictionary "direction". Als een van de knopjes in de dictionary ingedrukt wordt, geeft het de vector mee aan de Collision method
        foreach (Keys key in direction.Keys)
            if (inputHelper.KeyPressed(key)) 
                tetromino.Collision(grid.Grid, direction[key]);
        //Dit is voor debuggen. Als je E indrukt voeg je een tetromino toe aan de grid
        if (inputHelper.KeyPressed(Keys.E))
        {
            grid.Add(tetromino.Color, tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block);
            tetromino.Reset();
        }
        //Als de game in de Startup of Gameoverstate is, kan de speler spatiebalk indrukken om tetris (opnieuw) te starten.
        if ((gameState == GameState.Startup && inputHelper.KeyPressed(Keys.Space)) || (gameState == GameState.GameOver && inputHelper.KeyPressed(Keys.Space)))
        {
            gameState = GameState.Playing;
        }
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

        if (gameState == GameState.Startup)
        {

        }
        if (gameState == GameState.Playing)
        {

        }
        if (gameState == GameState.GameOver)
        {

        }
        spriteBatch.End();
    }

    public void Reset()
    {
        tetromino.Reset();
        grid.Clear();
    }
    public List<Tetromino> AddBag()
    {
		newBag.Clear();
        bagOfTetrominos = new List<Tetromino> { new I(), new J(), new L(), new O(), new S(), new T(), new Z() };
        while (bagOfTetrominos.Count > 0)
        {
            newBag.Add(bagOfTetrominos[random.Next(0, bagOfTetrominos.Count)]);
            bagOfTetrominos.Remove(newBag.Last());
        }
        return newBag;
    }

}
