using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

public class Game1 : Game
{
    private static GraphicsDeviceManager _graphics;
    public static SpriteBatch spriteBatch;
    public static GraphicsDevice graphicsDevice;

    private Map bgMap;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
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
        Console.WriteLine("Loading content");
        spriteBatch = new SpriteBatch(GraphicsDevice);
        graphicsDevice = base.GraphicsDevice;
        this.bgMap = new Map(new Size(300, 300), "Data/Texture/Test.png");
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        Console.WriteLine("Drawing...");
        spriteBatch.Begin();
        this.bgMap.Draw();
        Building.DrawAll();
        spriteBatch.End();
        //Unit.DrawAll();

        base.Draw(gameTime);
    }
}
