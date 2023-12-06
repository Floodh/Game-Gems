﻿using System;
using System.Net;
using System.Threading;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

public class GameWindow : Game
{
    public static Texture2D whitePixelTexture;

    public enum State
    {
        MainMenu,
        InGame,
        GameOver
    }
    private State state;
    public static Point windowSize = new Point(1920, 1080);
    public static GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public static SpriteBatch spriteBatchUi;
    public static GraphicsDevice graphicsDevice;
    public static MouseState contextMouseState;
    public static KeyboardState contextKeyboardState;


    public static bool interactingWithUI = false;
    public static bool interactingWithSelectableBuilding = false;
    public static bool interactingWithContextMenu = false;
    public static bool isInside = false;

    private bool InteractingWithUI { get { return this.buildingSelector.State != BuildingSelector.EState.NotVisible; } }
    private bool IsInside
    {
        get
        {
            return this.IsActive
                && contextMouseState.X >= 0 && contextMouseState.X < base.Window.ClientBounds.Width
                && contextMouseState.Y >= 0 && contextMouseState.Y < base.Window.ClientBounds.Height;
        }
    }

    private Cursor cursor;
    private Map map;
    private Level level;
    private Background background;

    private BuildingSelector buildingSelector;
    private ResourcesUi resourcesUi;
    private ContextMenu contextMenu;
    private MainMenu mainMenu;
    private GameOverScreen gameOverScreen;


    private Camera _camera;
    public static Matrix WorldTranslation { get; set; }
    public static float Time { get; set; }



    public GameWindow()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = windowSize.X,
            PreferredBackBufferHeight = windowSize.Y
        };
        Content.RootDirectory = "Content";
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += this.OnResize;
        Exiting += ThemePlayer.Dispose;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        Console.WriteLine("Initlizing...");
        base.Initialize();

        Save.Load();

    }

    protected override void LoadContent()
    {
        Console.WriteLine("Loading content...");
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteBatchUi = new SpriteBatch(GraphicsDevice);
        graphicsDevice = base.GraphicsDevice;

        whitePixelTexture = new Texture2D(base.GraphicsDevice, 1, 1);
        whitePixelTexture.SetData(new Color[] { Color.White });

        this.cursor = new(this);
        this.background = new Background(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, graphicsDevice);

        //this.map = new Map("Data/MapData/TwoSides.png");
        //Building.SetGrid(this.map.SourceImage);
        //this.level = new Level(this.map.SourceImage);
        //this.map.RenderGrid = true;
        var displaySize = new Size(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        this.buildingSelector = new BuildingSelector(displaySize);
        this.resourcesUi = new ResourcesUi(displaySize);
        this.contextMenu = new ContextMenu();
        this.mainMenu = new MainMenu(new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

        this.state = State.MainMenu;
        this.mainMenu.EnterState(MainMenu.State.Start);
        ThemePlayer.Load();
        EffectPlayer.Load();

        _ = new Booster();
        _ = new Cannon();
        _ = new Generator();
        _ = new Healer();
        _ = new Wall();

        _camera = new();
        WorldTranslation = Matrix.CreateTranslation(0f, 0f, 0f);


        // TODO: use this.Content to load your game content here


        //  REMOVE THIS, edventually
        ThemePlayer.Start_PlayTheme_MainMenu();
    }

    protected override void Update(GameTime gameTime)
    {
        Time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        contextKeyboardState = Keyboard.GetState();
        contextMouseState = Mouse.GetState();

        if (this.mainMenu.shouldQuit)
        {
            Exit();
        }


        // TODO: Add your update logic here
        //Console.WriteLine($"Mouse coords: {contextMouseState.X}, {contextMouseState.Y}");

        if (this.state == State.MainMenu)
        {
            if (mainMenu.state == MainMenu.State.Loading)
            {
                this.state = State.InGame;
                GameArguments arguments = this.mainMenu.GetGameArguments();
                this.map = new Map(arguments.mapPath);
                Building.SetGrid(this.map.SourceImage);
                this.level = new Level(arguments);
                this.mainMenu.EnterState(MainMenu.State.InActive);

                // Camera setup
                _camera.MapSize = this.map.MapSize;
                _camera.ViewportWidth = GameWindow.windowSize.X;
                _camera.ViewportHeight = GameWindow.windowSize.Y;
                foreach (Unit unit in Unit.allUnits)
                    if (unit.faction == Faction.Player)
                        _camera.UpdateCenter(unit.TargetPosition.ToVector2());

            }
            this.mainMenu.UpdateByMouse(contextMouseState);
            this.mainMenu.UpdateByKeyboard(contextKeyboardState);

        }
        else if (this.state == State.InGame)
        {
            if (isInside = IsInside)
            {
                this.buildingSelector.UpdateByMouse(contextMouseState);
                this.buildingSelector.UpdateByKeyboard(contextKeyboardState);

                if (!InteractingWithUI)
                {
                    interactingWithContextMenu = this.contextMenu.Update();

                    if (!interactingWithContextMenu)
                    {
                        interactingWithSelectableBuilding = Building.UpdateAllByMouse(contextMouseState);
                    }

                }

            }
            interactingWithUI = this.InteractingWithUI;
            level.MayTick();    //  performs all ticks            

            // Click confirm
            this.cursor.Update(contextMouseState, buildingSelector.State);
            if (!interactingWithUI && !interactingWithContextMenu && !interactingWithSelectableBuilding)
                if (contextMouseState.LeftButton == ButtonState.Pressed)
                    this.cursor.Play();

            // Camera and Input utility update
            InputManager.Update();
            _camera.UpdateCenterByInput(clampToMap: true);
            _camera.UpdateZoom(clampToMap: true);
            WorldTranslation = _camera.TranslationMatrix;

            if (this.level.IsGameOver)
                this.gameOverScreen ??= new GameOverScreen(Save.HighscoreNight, windowSize);
            this.gameOverScreen?.Update(contextMouseState);
            if (this.gameOverScreen != null)
            {
                //Console.WriteLine(this.gameOverScreen.shouldExitToMenu);
                if (this.gameOverScreen.shouldExitToMenu)
                {
                    this.level = null;
                    this.map = null;
                    this.mainMenu = new MainMenu(windowSize);
                    this.gameOverScreen = null;
                    Unit.allUnits.Clear();
                    Building.allBuildings.Clear();
                    this.state = State.MainMenu;
                    ThemePlayer.StopPlayingTheme();
                    ThemePlayer.Start_PlayTheme_MainMenu();
                    Sunlight.Mask = Color.LightGray;
                }
            }
        }

        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        //Console.WriteLine("Drawing...");
        spriteBatchUi.Begin();
        this.background.Draw();
        spriteBatchUi.End();

        spriteBatch.Begin(transformMatrix: WorldTranslation);
        spriteBatchUi.Begin();

        this.map?.Draw();

        Building.DrawAll();
        Unit.DrawAll();
        Projectile.DrawAll();
        Animation.DrawAll();

        if (this.state == State.MainMenu)
        {
            this.mainMenu.Draw();
        }
        else if (this.state == State.InGame)
        {
            this.level?.dayNightCycle?.Draw();
        }

        if (this.state == State.InGame)
        {
            this.resourcesUi.Draw();
            if (!InteractingWithUI)
                this.contextMenu.Draw();
            this.buildingSelector.Draw();
            this.gameOverScreen?.Draw();


            string tipText = "Hold \"Shift\" key to build";
            Vector2 textSize = ResourcesUi.FontSystem.GetFont(40).MeasureString(tipText);
            Vector2 textPos;
            textPos.X = (-textSize.X + windowSize.X) / 2;
            textPos.Y = windowSize.Y - textSize.Y - 40;

            spriteBatchUi.DrawString(ResourcesUi.FontSystem.GetFont(40) ,"Hold \"Shift\" key to build", textPos, Color.Cyan);
        }

        spriteBatch.End();
        spriteBatchUi.End();

        base.Draw(gameTime);
    }


    public void OnResize(Object sender, EventArgs e)
    {
        
        Console.WriteLine("Updating");
        windowSize = this.Window.ClientBounds.Size;
        if (this.background != null)
            this.background.windowSize = windowSize;
        this.level?.dayNightCycle.SetWindowSize(windowSize);
        this.mainMenu.OnResize(windowSize);
        this.gameOverScreen?.AdjustDrawArea(windowSize);

    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        if (this.level != null)
            Save.WriteToFile();
        base.OnExiting(sender, args);
    }


}
