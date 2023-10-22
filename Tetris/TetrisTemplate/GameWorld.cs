using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Linq;
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
    int score, frames;
    int[] pointsPerLine;
    Level level;

	static public Color EmptyCell {  get { return Color.White; } }
    
    IDictionary<Keys, Vector2> MovementKeys;
	
    SpriteFont font;
	Vector2 textPosition;
    Color textColour = Color.Black;
    
    double timeSinceLastTetrominoFall = 0, horizontalCooldown = 0, verticalCooldown = 0;
    public GameWorld()
    {
        random = new Random();
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        pointsPerLine = new int[]{0, 40, 100, 300, 1200};//An array for points per line. 
        grid = new TetrisGrid();
        level = new Level();
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

            //Als Z wordt ingedrukt draait de tetromino tegen de klok in. Als je X of Up indrukt draait het met de klok mee. 
			if (inputHelper.KeyPressed(Keys.Z))
				tetromino.Rotate(grid.Grid, false);
			if ((inputHelper.KeyPressed(Keys.X))|| inputHelper.KeyPressed(Keys.Up))
				tetromino.Rotate(grid.Grid, true);

            if ((inputHelper.KeyPressed(Keys.C)) && (holdKeyPressed == false))
                HoldTetromino();//HoldTetromino swapt de huidige tetromino met de tetromino in de hold slot
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
            score += pointsPerLine[grid.CheckFullRows(level)]*(level.CurrentLevel+1);
            //The score wordt berekent met de array PointsPerLine. Je krijgt 40, 100, 300 of 1200 punten voor 1, 2, 3 of 4 lijnen.
            //De method CheckFullRows checkt voor volle rijen, haalt ze weg en returnt hoeveel volle rijen er waren
            //Het roept ook de mthod in de Level class om te checken of het level 1 omhoog is gegaan.

            timeSinceLastTetrominoFall += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastTetrominoFall >= level.FallSpeed && gameState == GameState.Playing)
            {
                if (tetromino.Collision(grid.Grid, new Vector2(0, 1), tetromino.Block) == false)
                {
                    grid.AddToGrid(tetromino.Color, tetromino.HorizontalIndex, tetromino.VerticalIndex, tetromino.Block);
                    MakeNewTetromino();
                }
                timeSinceLastTetrominoFall = 0;
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
		textPosition.X = grid.Grid.GetLength(0) * grid.WidthEmptyCell + startingpointGrid.X + 5;//puts the Xposition of the text 5 pixels to the right of the grid
		textPosition.Y = 0;

        if (gameState == GameState.Startup)
        {
			spriteBatch.DrawString(font, "press Spacebar to start!", textPosition, Color.Blue);
            Spacing();//spacing puts the Yposition of Texposition down by y pixels. (default is 15)
		}
        if (gameState == GameState.Playing)
        {
			ghostTetromino.Draw(spriteBatch, 0.3f);
			tetromino.Draw(spriteBatch, 1);
            holdTetromino.Draw(spriteBatch, 1);
			upcomingTetrominos[0].Draw(spriteBatch, 1);
            Spacing();
            spriteBatch.DrawString(font, "Level:" + level.CurrentLevel, textPosition, textColour);
            Spacing();
			spriteBatch.DrawString(font, "Lines cleared: " + level.totalLines, textPosition, textColour);
            Spacing();
            spriteBatch.DrawString(font, "Score:" + score, textPosition, textColour);
		}
        if (gameState == GameState.GameOver)
        {
			spriteBatch.DrawString(font, "Game Over. press Spacebar to restart!", textPosition, textColour);
			Spacing();
			spriteBatch.DrawString(font, "Lines cleared: " + level.totalLines, textPosition, textColour);
			Spacing();
			spriteBatch.DrawString(font, "Score:" + score, textPosition, textColour);
		}
        spriteBatch.End();
    }
    private void Spacing(int y = 15)
    {
        textPosition.Y += y;
    }
    public void Reset()
    {
		grid.Reset();//Maakt grid leeg
        level.Reset();
        upcomingTetrominos.Clear(); //Leegt de wachtrij van tetrominos en hervult het. 
		upcomingTetrominos.AddRange(AddBag());
        MakeNewTetromino();
        timeSinceLastTetrominoFall = 0;
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
