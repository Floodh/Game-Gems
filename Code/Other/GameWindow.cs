﻿using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

public class GameWindow : Game
{
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

    private Map map;
    private Level level;
    private Background background;

    private BuildingSelector buildingSelector;
    private ResourcesUi resourcesUi;
    private ContextMenu contextMenu;

    public GameWindow()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = windowSize.X,
            PreferredBackBufferHeight = windowSize.Y
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += this.OnResize;
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

        this.background = new Background(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, graphicsDevice);

        Texture2D cursorTexture = Texture2D.FromFile(GameWindow.graphicsDevice, "Data/TextureSources/cursor2.png");
        Mouse.SetCursor(MouseCursor.FromTexture2D(cursorTexture, 0, 0));

        this.map = new Map("Data/MapData/TwoSides.png");
        Building.SetGrid(this.map.SourceImage);
        this.level = new Level(this.map.SourceImage);
        //this.map.RenderGrid = true;
        var displaySize = new Size(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        this.buildingSelector = new BuildingSelector(displaySize);
        this.resourcesUi = new ResourcesUi(displaySize);
        this.contextMenu = new ContextMenu();


       // this.bgMap = new Map("Data/MapData/OG.png");
        //  test
        Boulder boulder = new Boulder();
        boulder.Place(new Point(1, 0));
        Wall wall = new Wall();
        wall.Place(new Point(5,3));
        Cannon cannon = new Cannon();
        cannon.Place(new Point(3,3));
        Healer healer = new Healer();
        healer.Place(3,1);
        Generator generator = new();
        generator.Place(1,5);
        Booster booster = new();
        booster.Place(1,4);


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        this.KeyCheck(contextKeyboardState);

        contextKeyboardState = Keyboard.GetState();
        contextMouseState = Mouse.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || contextKeyboardState.IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        //Console.WriteLine($"Mouse coords: {contextMouseState.X}, {contextMouseState.Y}");

        if (isInside = IsInside)
        {
            // Console.WriteLine($"Mouse|x:{contextMouseState.X}, y:{contextMouseState.Y}");

            //Console.WriteLine("     Is inside!");
            Camera.UpdateByMouse(contextMouseState,graphics);
            Camera.UpdateByKeyboard(contextKeyboardState);

            this.buildingSelector.UpdateByMouse(contextMouseState);
            this.buildingSelector.UpdateByKeyboard(contextKeyboardState);   

            if(!InteractingWithUI)
            {
                this.contextMenu.Update();
                interactingWithContextMenu = this.contextMenu.UpdateByMouse(contextMouseState);

                if(!interactingWithContextMenu)
                {
                    interactingWithSelectableBuilding = Building.UpdateAllByMouse(contextMouseState);
                }

            }

            this.IsMouseVisible = buildingSelector.State != BuildingSelector.EState.PlacementPending;
        }


        interactingWithUI = this.InteractingWithUI;
        level.MayTick();    //  performs all ticks
        // if (Building.grid.hasUpdated && this.map.RenderGrid)    //  this is ugly, but it works
        // {
        //     Building.grid.hasUpdated = false;
        //     this.map.RenderGrid = true; //  force update
        // }

        

        base.Update(gameTime);
    }

    // TODO remove
    public static bool Key1Pressed = false;
    public static bool Key1Active = false;

    public static bool Key2Pressed = false;
     public static bool Key3Pressed = false;
    public static float Speed = 1f;

    public static bool Key4Pressed = false;
    public static bool Key4Active = false;

    protected void KeyCheck(KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.D1))
            Key1Pressed = true;
        else if(keyboardState.IsKeyUp(Keys.D1) && Key1Pressed)
        {
            Key1Active = !Key1Active;
            Key1Pressed = false;
        }

        if (keyboardState.IsKeyDown(Keys.D2))
            Key2Pressed = true;
        else if(keyboardState.IsKeyUp(Keys.D2) && Key2Pressed)
        {
            Speed--;
            Key2Pressed = false;
        }

        if (keyboardState.IsKeyDown(Keys.D3))
            Key3Pressed = true;
        else if(keyboardState.IsKeyUp(Keys.D3) && Key3Pressed)
        {
            Speed++;
            Key3Pressed = false;
        }

        if (keyboardState.IsKeyDown(Keys.D4))
            Key4Pressed = true;
        else if(keyboardState.IsKeyUp(Keys.D4) && Key4Pressed)
        {
            Key4Active = !Key4Active;
            Key4Pressed = false;
        }
    }

    // End TODO

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        //Console.WriteLine("Drawing...");
        spriteBatch.Begin();

        this.background.Draw();
        
        this.map.Draw();

        Building.DrawAll();
        Unit.DrawAll();
        Projectile.DrawAll();
        Animation.DrawAll();

        this.buildingSelector.Draw(spriteBatch);
        this.resourcesUi.Draw(spriteBatch);
        this.contextMenu.Draw();
        this.level?.dayNightCycle?.Draw();

        
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

    }


}
