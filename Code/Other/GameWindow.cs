﻿﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

public class GameWindow : Game
{
    public static GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public static GraphicsDevice graphicsDevice;
    public static MouseState contextMouseState;
    public static KeyboardState contextKeyboardState;


    public static bool interactingWithUI = false;
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
    private Ray ray;

    public GameWindow()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1920,
            PreferredBackBufferHeight = 1080
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

        this.ray = new Ray();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        contextKeyboardState = Keyboard.GetState();
        contextMouseState = Mouse.GetState();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || contextKeyboardState.IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        //Console.WriteLine($"Mouse coords: {contextMouseState.X}, {contextMouseState.Y}");

        if (isInside = IsInside)
        {
            //Console.WriteLine("     Is inside!");
            Camera.UpdateByMouse(contextMouseState,graphics);
            Camera.UpdateByKeyboard(contextKeyboardState);

            this.buildingSelector.UpdateByMouse(contextMouseState);
            this.buildingSelector.UpdateByKeyboard(contextKeyboardState);   

            if(!InteractingWithUI)
            {
                this.contextMenu.Update();
                bool mouseOnContextMenu = this.contextMenu.UpdateByMouse(contextMouseState);

                this.ray.UpdateByKeyboard(contextKeyboardState); 
                this.ray.UpdateByMouse(contextMouseState);

                if(!mouseOnContextMenu)
                {
                    Building.UpdateAllByMouse(contextMouseState);
                }

            }   
        }


        interactingWithUI = this.InteractingWithUI;
        level.MayTick();    //  performs all ticks
        // if (Building.grid.hasUpdated && this.map.RenderGrid)    //  this is ugly, but it works
        // {
        //     Building.grid.hasUpdated = false;
        //     this.map.RenderGrid = true; //  force update
        // }

        this.ray.Update(gameTime);
        

        base.Update(gameTime);
    }

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

        this.ray.Draw();
        
        spriteBatch.End();


        base.Draw(gameTime);
    }
    
    
    public void OnResize(Object sender, EventArgs e)
    {
        Console.WriteLine("Updating");
        if (this.background != null)
            this.background.windowSize = this.Window.ClientBounds.Size;
    }


}
