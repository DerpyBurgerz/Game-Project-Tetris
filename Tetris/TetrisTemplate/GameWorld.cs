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
    int linesCleared, score, level, frames;
    int[] pointsPerLine;

	static public Color EmptyCell {  get { return Color.White; } }
    
    IDictionary<Keys, Vector2> MovementKeys;
	
    SpriteFont font;
	Vector2 textPosition;
	int textSpacing;
    
    float elapsedTime = 0;
    double horizontalCooldown = 0, verticalCooldown = 0, difficulty;

    Texture2D bronze = TetrisGame.ContentManager.Load<Texture2D>("Bronze");
    Texture2D silver = TetrisGame.ContentManager.Load<Texture2D>("Silver");
    Texture2D gold = TetrisGame.ContentManager.Load<Texture2D>("Gold");
    Texture2D platinum = TetrisGame.ContentManager.Load<Texture2D>("Platinum");
    Texture2D diamond = TetrisGame.ContentManager.Load<Texture2D>("Diamond");
    Vector2 rankPosition = new Vector2(600, 400);
    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        textSpacing = 15;//verticale ruimte tussen teksten
        pointsPerLine = new int[]{0, 40, 100, 300, 1200};//An array for points per line. 
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
            {
                if ((inputHelper.KeyDown(key)) && (horizontalCooldown >= 0.08) && (MovementKeys[key].Y == 0))
                {
                    tetromino.Collision(grid.Grid, MovementKeys[key], tetromino.Block);
                    horizontalCooldown = 0;
                }
                else if ((inputHelper.KeyDown(key)) && (verticalCooldown >= 0.08) && (MovementKeys[key].Y > 0))
                {
					tetromino.Collision(grid.Grid, MovementKeys[key], tetromino.Block);
					verticalCooldown = 0;
				}
			}
			horizontalCooldown += gameTime.ElapsedGameTime.TotalSeconds;
            verticalCooldown += gameTime.ElapsedGameTime.TotalSeconds;
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
            score += pointsPerLine[grid.CheckFullRows()]*(level+1);

            //INSERT HIER CODE VOOR DE PUNTEN
            difficulty = 1 - (0.01 * grid.TotalLinesCleared);
            if (difficulty < 0.2) difficulty = 0.2; //om te voorkomen dat de snelheid onhoudbaar wordt 
            level = grid.TotalLinesCleared;

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Iedere elapsedTime
            if (elapsedTime >= difficulty && gameState == GameState.Playing)
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

            spriteBatch.DrawString(font, "Lines cleared: " + grid.TotalLinesCleared, textPosition, Color.Black);
            textPosition.Y += textSpacing;
            spriteBatch.DrawString(font, "Score:" + score, textPosition, Color.Black);
            textPosition.Y += textSpacing;
            spriteBatch.DrawString(font, "Level:" + level, textPosition, Color.Black);
            textPosition.Y += textSpacing;
            if (level >= 0 && level < 10)
            {
                spriteBatch.Draw(bronze, rankPosition, Color.White);
            }

            if (level >= 10 && level < 20)
            {
                spriteBatch.Draw(silver, rankPosition, Color.White);
            }

            if (level >= 20 &&  level < 30)
            {
                spriteBatch.Draw(gold, rankPosition, Color.White);
            }

            if (level >= 30 && level < 40)
            {
                spriteBatch.Draw(platinum, rankPosition, Color.White);
            }

            if (level >= 40)
            {
                spriteBatch.Draw(diamond, rankPosition, Color.White);
            }

            
		}
        if (gameState == GameState.GameOver)
        {
			spriteBatch.DrawString(font, "press Spacebar to start!", textPosition, Color.Blue);
		}
        spriteBatch.End();
    }
    public void Reset()
    {
		grid.Reset();//Maakt grid leeg
        upcomingTetrominos.Clear(); //Leegt de wachtrij van tetrominos en hervult het. 
		upcomingTetrominos.AddRange(AddBag());
        MakeNewTetromino();
        elapsedTime = 0;
        holdKeyPressed = false;
        verticalCooldown = 0;
        horizontalCooldown = 0;
        score = 0;
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
                //als er nog geen holdTetromino is wordt de huidige tetromino erin gezet.
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
