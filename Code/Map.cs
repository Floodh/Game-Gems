using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;


class Map
{

    enum TilesRGB : uint
    {
        Dirt = 0xFF663931,
        Grass = 0xFF4b692F,
        Water = 0xFF5b6ee1,
    }

    private const string DIRT_TEXTURE_PATH = "Data/Texture/Dirt.png";
    private const string GRASS_TEXTURE_PATH = "Data/Texture/Grass.png";
    private const string WATER_TEXTURE_PATH = "Data/Texture/Water.png";

    private const int mapPixelToTile_Multiplier = 2;  //  1 pixel = 2x2 tiles
    private const int mapPixelToTexturePixel_Multiplier = 16;

    private Bitmap mapImage;

    private Texture2D drawTexture;

    private Texture2D dirtTexture;
    private Texture2D grassTexture;
    private Texture2D waterTexture;

    private Size drawTextureSize;

    public Point drawOffset = Point.Zero;

    public Map(string path)
    {
        //this.drawOffset = new Point(-150, -200);

        
        //  load the map
        this.mapImage = new Bitmap(path);
        this.drawTextureSize = new Size(mapImage.Width * mapPixelToTexturePixel_Multiplier, mapImage.Height * mapPixelToTexturePixel_Multiplier);

        //  graphic libary stuff
        GraphicsDevice graphicsDevice = Game1.graphicsDevice;
        RenderTarget2D renderTargetIsAOffScreenBuffer = new RenderTarget2D(graphicsDevice, drawTextureSize.Width, drawTextureSize.Height, false, SurfaceFormat.Color, DepthFormat.None);
        SpriteBatch spriteBatch = Game1.spriteBatch;

        //  load the textures
        //this.drawTexture = new Texture2D(Game1.graphicsDevice, size.Width, size.Height);
        this.dirtTexture = Texture2D.FromFile(graphicsDevice, DIRT_TEXTURE_PATH);
        this.grassTexture = Texture2D.FromFile(graphicsDevice, GRASS_TEXTURE_PATH);
        this.waterTexture = Texture2D.FromFile(graphicsDevice, WATER_TEXTURE_PATH);
   
        spriteBatch.Begin();

            graphicsDevice.SetRenderTarget(renderTargetIsAOffScreenBuffer);
            for (int y = 0; y < this.mapImage.Width; y++)
            for (int x = 0; x < this.mapImage.Height; x++)
            {
                //Console.WriteLine($"x : {x}, y : {y}");
                TilesRGB argb = (TilesRGB)mapImage.GetPixel(x, y).ToArgb();
                Rectangle drawRect = new Rectangle(x * mapPixelToTexturePixel_Multiplier, y * mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier);
                switch (argb)
                {
                    case TilesRGB.Dirt:
                        spriteBatch.Draw(dirtTexture, drawRect, new Rectangle(0,0,16,16), Color.White);
                        break;
                    case TilesRGB.Grass:
                        spriteBatch.Draw(grassTexture, drawRect, new Rectangle(0,0,16,16), Color.White);
                        break;
                    case TilesRGB.Water:
                        spriteBatch.Draw(waterTexture, drawRect, new Rectangle(0,0,16,16), Color.White);
                        break;
                    default:
                        Console.WriteLine("Warning not a tile");
                        break;
                }
                //Console.WriteLine(argb);
                
            }

        spriteBatch.End();

        //  annoying syntax to transfer the data from screenBuffer to the texture
        System.IO.Stream stream = new System.IO.MemoryStream();
        renderTargetIsAOffScreenBuffer.SaveAsPng(stream, drawTextureSize.Width, drawTextureSize.Height);
        this.drawTexture = Texture2D.FromStream(graphicsDevice, stream);
        graphicsDevice.SetRenderTarget(null);   //  give back the rendering target

        

    }

    public void Draw()
    {
        Game1.spriteBatch.Draw(drawTexture, new Rectangle(drawOffset.X, drawOffset.Y, drawTextureSize.Width, drawTextureSize.Height), Color.White);
    }
}