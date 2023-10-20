﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
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

	Tetromino tetromino, ghostTetromino;
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
	Vector2 textPosition;
	int textSpacing;
    float elapsedTime = 0;
    double movementCooldown = 0;

    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();
		grid.Clear();
        gameState = GameState.Startup;
        //In deze dictionary staan de toetsen die je in kan drukken voor de beweging van de tetromino's, en de beweging die het doet als je die toets indrukt.
        direction = new Dictionary<Keys, Vector2>()
        {
            {Keys.Left, new Vector2(-1, 0) },
            {Keys.Right, new Vector2(1, 0) },
            {Keys.Down, new Vector2(0, 1) },
            //{Keys.Up, new Vector2(0, -1) },//deze key is voor debuggen
        };
		textSpacing = 15;
		upcomingTetrominos = new List<Tetromino>();
		newBag = new List<Tetromino>();
		bagOfTetrominos = new List<Tetromino>();
		upcomingTetrominos.AddRange(AddBag());
        ghostTetromino = new Tetromino(Color.White);
	}

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
		
		
		if (gameState == GameState.Playing)
		{
			//Dit loopt door de dictionary "direction". Als een van de knopjes in de dictionary ingedrukt wordt, geeft het de vector mee aan de Collision method
			foreach (Keys key in direction.Keys)
				if ((inputHelper.KeyDown(key)) && (movementCooldown >=0.07))
				{
                    tetromino.Collision(grid.Grid, direction[key], tetromino.Block);
                    movementCooldown = 0;
				}
            //spatiebalk voor hard drop
			if (inputHelper.KeyPressed(Keys.Space))
			{
				while (tetromino.Collision(grid.Grid, new Vector2(0, 1), tetromino.Block)) ;
				grid.Add(tetromino.Color, tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block);
				NewTetromino();
            }
			//Dit is voor debuggen. Als je E indrukt voeg je een tetromino toe aan de grid
			if (inputHelper.KeyPressed(Keys.E))
            {
                grid.Add(tetromino.Color, tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block);
                NewTetromino();
            }

			if ((inputHelper.KeyPressed(Keys.Q)) || (inputHelper.KeyPressed(Keys.Z)) )
			{
				tetromino.Rotate(grid.Grid, true);
			}
			if ((inputHelper.KeyPressed(Keys.X))|| inputHelper.KeyPressed(Keys.Up))
			{
				tetromino.Rotate(grid.Grid, false);
			}
            //movementcooldown is er zodat je pijltjestoetsen ingedruk kan houden om naar links, rechts of beneden te gaan.
			movementCooldown += gameTime.ElapsedGameTime.TotalSeconds;
            //De ghostTetromino krijg de positie, array en kleur van de tetromino in het speelveld.
            ghostTetromino.GhostUpdate(tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block, tetromino.Color);
            //Deze while loop verplaatst de ghostTetromino naar beneden tot het niet meer naar beneden kan.
			while (ghostTetromino.Collision(grid.Grid, new Vector2(0, 1), ghostTetromino.Block));

		}
        
		//Als de game in de Startup of Gameoverstate is, kan de speler spatiebalk indrukken om tetris (opnieuw) te starten.
		if ((gameState == GameState.Startup && inputHelper.KeyPressed(Keys.Space)) || (gameState == GameState.GameOver && inputHelper.KeyPressed(Keys.Space)))
        {
            gameState = GameState.Playing;
			Reset();
        }
    }

    public void Update(GameTime gameTime)
    {

        //grid.Add(tetromino.Color, new Vector2(5, 4), tetromino.Block);
        grid.CheckFullRows();
    
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (elapsedTime >= 1 && gameState == GameState.Playing)
        {
			if (tetromino.Collision(grid.Grid, new Vector2(0, 1), tetromino.Block) == false)
			{
				grid.Add(tetromino.Color, tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block);
				NewTetromino();
			}
            elapsedTime = 0;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        grid.Draw(gameTime, spriteBatch);
		//spriteBatch.DrawString(font, "Hello!", Vector2.Zero, Color.Blue);
		textPosition.X = grid.Grid.GetLength(0) * grid.WidthEmptyCell;
		textPosition.Y = 0;

        if (gameState == GameState.Startup)
        {
			spriteBatch.DrawString(font, "press Spacebar to start!", textPosition, Color.Blue);
			textPosition.Y += textSpacing;
			spriteBatch.DrawString(font, "yay", textPosition, Color.Blue);
		}
        if (gameState == GameState.Playing)
        {
			ghostTetromino.Draw(spriteBatch, 0.3f);
			tetromino.Draw(spriteBatch, 1);
			upcomingTetrominos[0].Draw(spriteBatch, 1);
		}
        if (gameState == GameState.GameOver)
        {
			spriteBatch.DrawString(font, "press Spacebar to start!", textPosition, Color.Blue);
		}
        spriteBatch.End();
    }

    public void Reset()
    {
		NewTetromino();
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
	public void NewTetromino()
	{
		tetromino = upcomingTetrominos[0];
        ghostTetromino.GhostUpdate(tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block, tetromino.Color);
		upcomingTetrominos.RemoveAt(0);
		tetromino.Reset();
		if (upcomingTetrominos.Count <= 2)
			upcomingTetrominos.AddRange(AddBag());
		if (tetromino.Collision(grid.Grid, new Vector2(0, 0), tetromino.Block) == false)
		{
			gameState = GameState.GameOver;
		}
	}

}
