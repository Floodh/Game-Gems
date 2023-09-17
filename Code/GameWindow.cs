﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

public class GameWindow : Game
{
    public static GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public static GraphicsDevice graphicsDevice;

    private Map bgMap;


    public GameWindow()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1920,
            PreferredBackBufferHeight = 1080
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
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
        this.bgMap = new Map("Data/MapData/OG.png");
        
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

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        Camera.UpdateByMouse(Mouse.GetState(),graphics);
        Camera.UpdateByKeyboard(Keyboard.GetState());

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        //Console.WriteLine("Drawing...");
        spriteBatch.Begin();
        
        this.bgMap.Draw();

        Building.DrawAll();
        spriteBatch.End();
        //Unit.DrawAll();

        base.Draw(gameTime);
    }
}
