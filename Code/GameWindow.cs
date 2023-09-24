using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

public class GameWindow : Game
{
    public static GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public static GraphicsDevice graphicsDevice;

    private Map map;
    private Level level;


    public GameWindow()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 1040
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

        this.map = new Map("Data/MapData/OG.png");
        Building.SetGrid(this.map.SourceImage);
        Camera.Init(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        this.level = new Level(this.map.SourceImage);
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        Camera.UpdateByMouse(Mouse.GetState());
        Camera.UpdateByKeyboard(Keyboard.GetState());

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        //Console.WriteLine("Drawing...");
        spriteBatch.Begin();
        
        this.map.Draw();

        Building.DrawAll();
        spriteBatch.End();
        //Unit.DrawAll();

        base.Draw(gameTime);
    }
}
