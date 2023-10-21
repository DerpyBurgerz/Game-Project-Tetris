using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

//using TetrisTemplate;

/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    enum GameState
    {
        Playing,
        GameOver,
        Startup
    }
	GameState gameState;

	Tetromino tetromino, ghostTetromino, holdTetromino, swapTetromino;
    List <Tetromino> upcomingTetrominos, bagOfTetrominos, newBag;
    bool holdKeyPressed;
    public static Random Random { get { return random; } }
    static Random random;
    
    TetrisGrid grid;
    static Vector2 startingpointGrid;
    static public Vector2 StartingpointGrid { get { return startingpointGrid; } }
    int linesCleared;
    static public Color EmptyCell {  get { return Color.White; } }
    
    IDictionary<Keys, Vector2> MovementKeys;
	
    SpriteFont font;
	Vector2 textPosition;
	int textSpacing;
    
    float elapsedTime = 0;
    double movementCooldown = 0;
    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
		textSpacing = 15;//verticale ruimte tussen teksten

		grid = new TetrisGrid();
        startingpointGrid = new Vector2(TetrisGame.ScreenSize.X / 2 - TetrisGrid.Width/2 * grid.WidthEmptyCell, 0);
        gameState = GameState.Startup;
        //In deze dictionary staan de toetsen die je in kan drukken voor de beweging van de tetromino's, en de beweging die het doet als je die toets indrukt.
        MovementKeys = new Dictionary<Keys, Vector2>()
        {
            {Keys.Left, new Vector2(-1, 0) },
            {Keys.Right, new Vector2(1, 0) },
            {Keys.Down, new Vector2(0, 1) },
            //{Keys.Up, new Vector2(0, -1) },//deze key is voor debuggen
        };
		upcomingTetrominos = new List<Tetromino>();//Deze lijst is een soort wachtrij voor de tetromino's.
		newBag = new List<Tetromino>();//Deze lijst is voor het maken van de wachtrij
		bagOfTetrominos = new List<Tetromino>();//Deze lijst is ook voor het maken van de wachtrij.
		upcomingTetrominos.AddRange(AddBag());//de Method AddBagg geeft de 7 tetromino's in een willekeurige volgorde terug.
        holdTetromino = new Tetromino(Color.White);//De holdtetromino kan geswapt worden met de tetromino in het speelveld
        ghostTetromino = new Tetromino(Color.White);//De ghostTetromino is de tetromino die je onderaan het scherm ziet.
                                                    //Deze laat zien waar je tetromino landt als je een hard drop doet.
	}
    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {	
		if (gameState == GameState.Playing)
		{
			//Dit loopt door de dictionary "direction". Als een van de knopjes in de dictionary ingedrukt wordt, geeft het de vector mee aan de Collision method
			foreach (Keys key in MovementKeys.Keys)
				if ((inputHelper.KeyDown(key)) && (movementCooldown >=0.07))
				{
                    tetromino.Collision(grid.Grid, MovementKeys[key], tetromino.Block);
                    movementCooldown = 0;
				}
            //spatiebalk voor hard drop
			if (inputHelper.KeyPressed(Keys.Space))
			{
				while (tetromino.Collision(grid.Grid, new Vector2(0, 1), tetromino.Block)) ;
				grid.AddToGrid(tetromino.Color, tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block);
				MakeNewTetromino();
            }
			//Dit is voor debuggen. Als je E indrukt voeg je een tetromino toe aan de grid
			if (inputHelper.KeyPressed(Keys.E))
            {
                grid.AddToGrid(tetromino.Color, tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block);
                MakeNewTetromino();
            }

            //Als Q of Z wordt ingedrukt draait de tetromino met de klok mee. ALs je X of Up indrukt draait het tegen de klok in. 
			if ((inputHelper.KeyPressed(Keys.Q)) || (inputHelper.KeyPressed(Keys.Z)) )
				tetromino.Rotate(grid.Grid, false);
			if ((inputHelper.KeyPressed(Keys.X))|| inputHelper.KeyPressed(Keys.Up))
				tetromino.Rotate(grid.Grid, true);

            if ((inputHelper.KeyPressed(Keys.C)) && (holdKeyPressed == false))
                HoldTetromino();//HoldTetromino swapt de huidige tetromino met de tetromino in de hold slot

            //movementcooldown is er zodat je pijltjestoetsen ingedruk kan houden om naar links, rechts of beneden te gaan.
            movementCooldown += gameTime.ElapsedGameTime.TotalSeconds;
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
        if (gameState == GameState.Playing)
        {
            linesCleared = grid.CheckFullRows();

            //INSERT HIER CODE VOOR DE PUNTEN

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Iedere elapsedTime
            if (elapsedTime >= 1 && gameState == GameState.Playing)
            {
                if (tetromino.Collision(grid.Grid, new Vector2(0, 1), tetromino.Block) == false)
                {
                    grid.AddToGrid(tetromino.Color, tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block);
                    MakeNewTetromino();
                }
                elapsedTime = 0;
            }
            //De ghostTetromino krijg de positie, array en kleur van de tetromino in het speelveld.
            ghostTetromino.GhostUpdate(tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block, tetromino.Color);

            //Deze while loop verplaatst de ghostTetromino naar beneden tot het niet meer naar beneden kan.
            while (ghostTetromino.Collision(grid.Grid, new Vector2(0, 1), ghostTetromino.Block)) ;
        }
	}

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        grid.Draw(gameTime, spriteBatch);
		//spriteBatch.DrawString(font, "Hello!", Vector2.Zero, Color.Blue);
		textPosition.X = grid.Grid.GetLength(0) * grid.WidthEmptyCell + startingpointGrid.X;
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
            holdTetromino.Draw(spriteBatch, 1);
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
		grid.Clear();//Maakt grid leeg
        //Leegt de wachtrij van tetrominos en hervult het. 
        upcomingTetrominos.Clear();
		upcomingTetrominos.AddRange(AddBag());
        MakeNewTetromino();
        elapsedTime = 0;
        holdKeyPressed = false;
    }
    public List<Tetromino> AddBag()
    {
		newBag.Clear();
        //In deze lijst worden de 7 tetromino's gezet.
        bagOfTetrominos = new List<Tetromino> { new I(), new J(), new L(), new O(), new S(), new T(), new Z() };
        //Deze loop blijft oorgaan tot bagOfTetromninos leeg is.
        while (bagOfTetrominos.Count > 0)
        {
            //Er wordt vanuit bagOfTetrominos een random Tetromino in newBag gezet, en deze wordt daarna uit bagOfTetrominos gehaald.
            newBag.Add(bagOfTetrominos[random.Next(0, bagOfTetrominos.Count)]);
            bagOfTetrominos.Remove(newBag.Last());
        }
        return newBag;
    }
	public void MakeNewTetromino()
	{
		//tetromino wordt de eerste tetromino in de wachtrij, en deze wordt uit de wachtrij gehaald
		tetromino = upcomingTetrominos[0];			  
		upcomingTetrominos.RemoveAt(0);
		tetromino.Reset();//positie van tetromino wordt bovenaan het scherm gezet.
		//ghostTetromino krijgt de positie, orientatie en kleur van de nieuw aangemaakte tetromino.
		ghostTetromino.GhostUpdate(tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block, tetromino.Color);
        //Als er te weinig tetrominos zijn in de wachtrij wordt het bijgevuld.
		if (upcomingTetrominos.Count <= 2)
			upcomingTetrominos.AddRange(AddBag());
        //Als de nieuw aangemaakt tetromino niet op het scherm kan, is het gameover.
		if (tetromino.Collision(grid.Grid, new Vector2(0, 0), tetromino.Block) == false)
			gameState = GameState.GameOver;
        holdKeyPressed = false;
	}
    public void HoldTetromino()
    {
		if (holdTetromino.Color == Color.White)
		{
			if ((holdTetromino.Collision(grid.Grid, new Vector2(0, 0), holdTetromino.Block)) == true)
			{
				holdTetromino = tetromino;
				MakeNewTetromino();
			}
		}
		else
		{
            //deze code swapt de tetromino en de hold piece
            swapTetromino = tetromino;
			tetromino = holdTetromino;
			holdTetromino = swapTetromino;
			swapTetromino = null;
            tetromino.Reset();
		}
        holdTetromino.Reset();//reset function om de draaiing te resetten
		holdTetromino.SetPosition(-5, 4);//positie van de hold piece
		holdKeyPressed = true;
	}
}
