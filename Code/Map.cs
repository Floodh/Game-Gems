using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

class Map
{

    private const string GRASS_TEXTURE_PATH = "Data/Texture/Grass.png";

    private const int pixelToTile_Multiplier = 2;  //  1 pixel = 2x2 tiles
    private const int pixelToTexturePixel_Multiplier = 32;

    private Texture2D drawTexture;
    private Texture2D grassTexture;
    private Size size;

    public Map(string path)
    {
        this.size = new Size(Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);

        GraphicsDevice graphicsDevice = Game1.graphicsDevice;
        RenderTarget2D renderTargetIsAOffScreenBuffer = new RenderTarget2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
        SpriteBatch spriteBatch = Game1.spriteBatch;

        //  load the textures
        //this.drawTexture = new Texture2D(Game1.graphicsDevice, size.Width, size.Height);
        this.grassTexture = Texture2D.FromFile(graphicsDevice, GRASS_TEXTURE_PATH);
        
        spriteBatch.Begin();

            graphicsDevice.SetRenderTarget(renderTargetIsAOffScreenBuffer);
            for (int y = 0; y < this.size.Width; y+=pixelToTexturePixel_Multiplier)
            for (int x = 0; x < this.size.Height; x+=pixelToTexturePixel_Multiplier)
            {
                spriteBatch.Draw(grassTexture, new Rectangle(x, y, pixelToTexturePixel_Multiplier, pixelToTexturePixel_Multiplier), new Rectangle(16,0,16,16), Color.GreenYellow);
            }

        spriteBatch.End();

        //  annoying syntax to transfer the data from screenBuffer to the texture
        System.IO.Stream stream = new System.IO.MemoryStream();
        renderTargetIsAOffScreenBuffer.SaveAsPng(stream, size.Width, size.Height);
        this.drawTexture = Texture2D.FromStream(graphicsDevice, stream);
        graphicsDevice.SetRenderTarget(null);   //  give back the rendering target

        

    }

    public void Draw()
    {
        Game1.spriteBatch.Draw(drawTexture, new Rectangle(0, 0, size.Width, size.Height), Color.White);
    }
}