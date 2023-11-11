﻿using System;
using System.Net;
using System.Threading;
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
    public static GraphicsDevice graphicsDevice;
    public static MouseState contextMouseState;
    public static KeyboardState contextKeyboardState;


    public static bool interactingWithUI = false;
    public static bool interactingWithSelectableBuilding = false;
    public static bool interactingWithContextMenu = false;
    public static bool isInside = false;

    private bool InteractingWithUI {get{return this.buildingSelector.State != BuildingSelector.EState.NotVisible;}}
    private bool IsInside { get 
    {   return this.IsActive
            && contextMouseState.X >= 0 && contextMouseState.X < base.Window.ClientBounds.Width
            && contextMouseState.Y >= 0 && contextMouseState.Y < base.Window.ClientBounds.Height;
    }}

    private Cursor cursor;
    private Map map;
    private Level level;
    private Background background;

    private BuildingSelector buildingSelector;
    private ResourcesUi resourcesUi;
    private ContextMenu contextMenu;
    private MainMenu mainMenu;

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
        
    }

    protected override void LoadContent()
    {
        Console.WriteLine("Loading content...");
        spriteBatch = new SpriteBatch(GraphicsDevice);
        graphicsDevice = base.GraphicsDevice;
        
        whitePixelTexture = new Texture2D(base.GraphicsDevice, 1, 1);
        whitePixelTexture.SetData( new Color[] { Color.White });

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

        _ = new Booster();
        _ = new Cannon();
        _ = new Generator();
        _ = new Healer();
        _ = new Wall();

        // TODO: use this.Content to load your game content here


        //  REMOVE THIS, edventually
            ThemePlayer.Start_PlayTheme_MainMenu();
    }

    protected override void Update(GameTime gameTime)
    {
        contextKeyboardState = Keyboard.GetState();
        contextMouseState = Mouse.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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
            }
            this.mainMenu.UpdateByMouse(contextMouseState);
            this.mainMenu.UpdateByKeyboard(contextKeyboardState);
            
        }
        else if (this.state == State.InGame)
        {
            if (isInside = IsInside)
            {

                Camera.UpdateByMouse(contextMouseState,graphics);
                Camera.UpdateByKeyboard(contextKeyboardState);

                this.buildingSelector.UpdateByMouse(contextMouseState);
                this.buildingSelector.UpdateByKeyboard(contextKeyboardState);   

                if(!InteractingWithUI)
                {
                    interactingWithContextMenu = this.contextMenu.Update(contextMouseState);

                    if(!interactingWithContextMenu)
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
                if(contextMouseState.LeftButton == ButtonState.Pressed)
                    this.cursor.Play();    

        }



        // if (Building.grid.hasUpdated && this.map.RenderGrid)    //  this is ugly, but it works
        // {
        //     Building.grid.hasUpdated = false;
        //     this.map.RenderGrid = true; //  force update
        // }

        

        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        //Console.WriteLine("Drawing...");
        spriteBatch.Begin();

        this.background.Draw();
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
            this.resourcesUi.Draw(spriteBatch);
            if(!InteractingWithUI)
                this.contextMenu.Draw();
             this.buildingSelector.Draw();
            this.level?.dayNightCycle?.Draw();            
        }

        spriteBatch.End();
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

    }



}
